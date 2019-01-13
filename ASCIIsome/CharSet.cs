using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Xml;
using System.Xml.Schema;
using ASCIIsome.Properties;

namespace ASCIIsome
{
    [Serializable]
    public class CharSet : Dictionary<double, char>, IComparable<CharSet>, IEquatable<CharSet>, ICloneable
    {
        private const double minimalGrayscaleDivision = 1E-5;
        private static readonly Range<double> defaultGrayscaleRange = new Range<double>(0.00000, 1.00000);
        private const string defaultCharSetFileName = "cs_ASCIISymbols";
        public static string CharSetFolderPath { get; } = Path.Combine(ApplicationInfo.AppDataFolder, "CharSets");
        public static CharSet Default { get; } = ParseFromXmlFile(typeof(CharSet).Assembly.GetManifestResourceStream($"ASCIIsome.Resources.CharSets.{defaultCharSetFileName}.xml"), false);
        public string DisplayName { get; set; }
        public override string ToString() => DisplayName;

        public static CharSet Concat(List<string> filenames)
        {
            List<CharSet> parsedCharSets = new List<CharSet>();
            foreach (string filename in filenames)
            {
                string filePath = Path.Combine(CharSetFolderPath, filename);
                parsedCharSets.Add(ParseFromXmlFile(filePath));
            }
            return Concat(parsedCharSets.ToArray());
        }

        private static CharSet Concat(params CharSet[] charSets)
        {
            switch (charSets.Length)
            {
                case 0:
                    return null;
                case 1:
                    return charSets[0];
                default:
                    CharSet charSetsConcatenated = new CharSet { DisplayName = $"{charSets.Length} charsets concatenated" };
                    foreach (CharSet charSet in charSets)
                    {
                        foreach (KeyValuePair<double, char> charInfo in charSet)
                        {
                            try
                            {
                                charSetsConcatenated.Add(charInfo.Key, charInfo.Value);
                            }
                            catch (Exception) // [HV] In case that Key already exists, thus make KeyValuePairs distinct (in term of keys)
                            {
                                continue;
                            }
                        }
                    }
                    return MakeOrderedDistinct(charSetsConcatenated);
            }
        }

        private static CharSet MakeOrderedDistinct(CharSet charSet) // [HV] Is this necessary in practical use? 

        //IEnumerable<KeyValuePair<double, char>> orderedDistinctCharInfo = charSet.Distinct().OrderBy(x => x.Key);
        //if (charSet.SequenceEqual(orderedDistinctCharInfo))
        //{
        //    return charSet;
        //}
        //CharSet orderedDistinctCharSet = new CharSet();
        //foreach (KeyValuePair<double, char> charInfo in orderedDistinctCharInfo)
        //{
        //    orderedDistinctCharSet.Add(charInfo.Key, charInfo.Value);
        //}
        //orderedDistinctCharSet.DisplayName = charSet.DisplayName;
        //return orderedDistinctCharSet;

        => charSet;

        private new void Add(double key, char value)
        {
            if (!ContainsValue(value))
            {
                while (key <= defaultGrayscaleRange.Maximum)
                {
                    try
                    {
                        base.Add(key, value);
                        break;
                    }
                    catch (ArgumentException)
                    {
                        // WARNING: [HV] This particular approach can theoretically cause significant difference between the actual value of grayscale 
                        // index of a character and the value assigned when being added into a charset, in case that all neighboring keys have been occupied. 
                        // OR, if all possible values of key between it's original grayscale index and DefaultGrayscaleRange.Maximum have already been occupied, 
                        // the character will be discarded instead of being added to the charset, which may be unexpected to the end user

                        key += minimalGrayscaleDivision;
                    }
                }
            }
        }
        private void Add(KeyValuePair<double, char> charInfo) => Add(charInfo.Key, charInfo.Value);

        public object Clone()
        {
            CharSet newCharSet = new CharSet { DisplayName = DisplayName };
            foreach (KeyValuePair<double, char> charInfo in this)
            {
                newCharSet.Add(charInfo);
            }
            return newCharSet;
        }

        public static CharSet operator |(CharSet left, CharSet right) => Concat(left, right);
        public static CharSet operator &(CharSet left, CharSet right)
        {
            switch (left.CompareTo(right))
            {
                case -1:
                case 0:
                    return left;
                case 1:
                    return right;
                default:
                    CharSet basis = left.Count < right.Count ? left.Clone() as CharSet : right.Clone() as CharSet;
                    CharSet other = left.Count < right.Count ? right : left;
                    foreach (KeyValuePair<double, char> charInfo in basis)
                    {
                        if (!other.Contains(charInfo))
                        {
                            basis.Remove(charInfo.Key);
                        }
                    }
                    return basis;
            }
        }

        public static CharSet ParseFromXmlFile(dynamic source, bool validate = true) // TODO: [HV] Exception handling is still needed in external code to prompt user about any failure occurred
        {
            CharSet parsedCharSet = new CharSet();
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(XmlSchema.Read(typeof(CharSet).Assembly.GetManifestResourceStream("ASCIIsome.Resources.CharSets.CharSetSchema.xsd"), (sender, e) => throw e.Exception));
            XmlDocument document = new XmlDocument { XmlResolver = new XmlSecureResolver(new XmlUrlResolver(), typeof(CharSet).Assembly.Evidence), Schemas = schemaSet };
            document.Load(source);
            if (validate)
            {
                document.Validate((sender, e) => throw e.Exception);
            }
            XmlNode rootNode = document.DocumentElement;
            XmlNodeList nodeList = rootNode.ChildNodes;
            foreach (XmlNode xmlNode in nodeList)
            {
                double parsedGrayscaleIndex = double.Parse(xmlNode.Attributes["GrayscaleIndex"].Value, CultureInfo.InvariantCulture.NumberFormat);
                char parsedCharacter = char.Parse(xmlNode.Attributes["Character"].Value);
                parsedCharSet.Add(parsedGrayscaleIndex, parsedCharacter);
            }
            parsedCharSet.DisplayName = rootNode.Attributes["DisplayName"].Value;
            return MakeOrderedDistinct(parsedCharSet);
        }

        public void ExportToXmlFile(string filePath)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings { Indent = true };
            using (XmlWriter xmlWriter = XmlWriter.Create(filePath, xmlWriterSettings))
            {
                xmlWriter.WriteStartElement("CharSet");
                xmlWriter.WriteAttributeString("DisplayName", DisplayName);
                xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                xmlWriter.WriteAttributeString("xsi", "schemaLocation", null, "ASCIIsome.Resources.CharSets CharSetSchema.xsd");
                CharSet orderedDistinctCharSet = MakeOrderedDistinct(this);
                foreach (KeyValuePair<double, char> charInfo in orderedDistinctCharSet)
                {
                    xmlWriter.WriteStartElement("CharInfo");
                    xmlWriter.WriteAttributeString("GrayscaleIndex", charInfo.Key.ToString(CultureInfo.InvariantCulture));
                    xmlWriter.WriteAttributeString("Character", charInfo.Value.ToString(CultureInfo.InvariantCulture));
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
        }

        [Obsolete("This method is for debug use only. Remove calls to it in production code. ")]
        public void DebugEnumerateKeyValuePairs()
        {
            foreach (KeyValuePair<double, char> charInfo in this)
            {
                Debug.WriteLine(charInfo.Key + ", " + charInfo.Value);
            }
        }

        public static IEnumerable<string> EnumerateFiles() => Directory.GetFiles(CharSetFolderPath).Where(s => s.EndsWith(".xml", StringComparison.InvariantCulture));
        public static IEnumerable<(string displayName, string filename)> GetDisplayNames(IEnumerable<string> filenames)
        {
            foreach (string filename in filenames)
            {
                XmlDocument document = new XmlDocument { XmlResolver = new XmlSecureResolver(new XmlUrlResolver(), typeof(CharSet).Assembly.Evidence) };
                string displayName;
                try
                {
                    document.Load(filename);
                    displayName = document.DocumentElement.Attributes["DisplayName"].Value;
                }
                catch (Exception)
                {
                    continue;
                }
                yield return (displayName, filename);
            }
        }

        public static void InitializeCharSetFolder()
        {
            if (!Directory.Exists(CharSetFolderPath))
            {
                Directory.CreateDirectory(CharSetFolderPath);
                ResourceSet resourceSet = Resources.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
                foreach (DictionaryEntry resourceEntry in resourceSet)
                {
                    string resourceName = resourceEntry.Key.ToString();
                    if (resourceName.StartsWith("cs_", StringComparison.InvariantCulture))
                    {
                        using (Stream resourceStream = typeof(CharSet).Assembly.GetManifestResourceStream($"ASCIIsome.Resources.CharSets.{resourceName}.xml"))
                        {
                            using (FileStream fileStream = new FileStream($"{CharSetFolderPath}\\{resourceName}.xml", FileMode.Create, FileAccess.Write))
                            {
                                resourceStream.CopyTo(fileStream);
                            }
                        }
                    }
                }
            }
        }

        public bool Contains(CharSet other) => other.All(v => this.Contains(v));
        public int CompareTo(CharSet other)
        {
            if (Contains(other))
            {
                if (other.Contains(this))
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (other.Contains(this))
                {
                    return -1;
                }
                else
                {
                    return int.MinValue;
                }
            }
        }
        public static bool operator <(CharSet left, CharSet right) => left.CompareTo(right) < 0;
        public static bool operator <=(CharSet left, CharSet right) => left.CompareTo(right) <= 0;
        public static bool operator >(CharSet left, CharSet right) => left.CompareTo(right) > 0;
        public static bool operator >=(CharSet left, CharSet right) => left.CompareTo(right) >= 0;

        public bool Equals(CharSet other)
        {
            if (ReferenceEquals(this, other) || this is null && other is null)
            {
                return true;
            }
            else if (this is null || other is null)
            {
                return false;
            }
            else
            {
                return CompareTo(other) == 0;
            }
        }
        public override bool Equals(object obj) => obj is CharSet ? Equals(obj as CharSet) : false;
        public static bool operator ==(CharSet left, CharSet right) => left.Equals(right);
        public static bool operator !=(CharSet left, CharSet right) => !left.Equals(right);

        public bool IdenticalTo(CharSet other) => Equals(other) && DisplayName == other.DisplayName;
        public override int GetHashCode()
        {
            // [HV] Here flagging is used to make identical CharSets give the same hash code. For example, in all characters in a CharSet instance, 
            // including its DisplayName and all CharInfo KeyValuePairs, if a '\u0000' presents, the last bit of the hash code having a value of 1 << 0
            // (i.e. 1) will be 1, otherwise 0. Similarly if a '\u0001' presents determines the second last bit having a value of 1 << 1 (i.e. 2) and so 
            // on. However, same hash code does not necessarily mean identical CharSets as we are mapping more (infinite possibilities of contents in a 
            // CharSet) to less (4294967296 finite possibilities of the value of the hash code), and there are obvious flaws such as even number of the
            // same character in the DisplayName or elsewhere cancel out the flag for one another, making it equivalent to none. 

            int hashCode = 0;
            unchecked
            {
                foreach (char character in DisplayName)
                {
                    hashCode += 1 << character;
                }
                foreach (KeyValuePair<double, char> charInfo in this)
                {
                    hashCode += 1 << charInfo.Value;
                }
            }
            return hashCode;
        }
    }
}
