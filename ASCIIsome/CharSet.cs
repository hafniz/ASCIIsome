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
    public class CharSet : Dictionary<double, char>, IEquatable<CharSet>
    {
        private const double minimalGrayscaleDivision = 1E-5;
        private static readonly Range<double> defaultGrayscaleRange = new Range<double>(0.00000, 1.00000);
        private static readonly string charSetFolderPath = Path.Combine(ApplicationInfo.AppDataFolder, "CharSets");
        private const string defaultCharSetFileName = "cs_ASCIISymbols";
        public static CharSet Default { get; } = ParseFromXmlFile(typeof(CharSet).Assembly.GetManifestResourceStream($"ASCIIsome.Resources.CharSets.{defaultCharSetFileName}.xml"), false);
        public string DisplayName { get; set; }
        public override string ToString() => DisplayName;

        public static CharSet Concat(List<string> filenames)
        {
            List<CharSet> parsedCharSets = new List<CharSet>();
            foreach (string filename in filenames)
            {
                string filePath = Path.Combine(charSetFolderPath, filename);
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
                            charSetsConcatenated.Add(charInfo.Key, charInfo.Value);
                        }
                    }
                    return MakeOrderedDistinct(charSetsConcatenated);
            }
        }
        public static CharSet operator |(CharSet charSet1, CharSet charSet2) => Concat(charSet1, charSet2);

        private static CharSet MakeOrderedDistinct(CharSet charSet) // [HV] Is this necessary in practical use? 
        {
            //IEnumerable<KeyValuePair<double, char>> orderedDistinctKeyValuePairs = charSet.Distinct().OrderBy(x => x.Key);
            //if (charSet.SequenceEqual(orderedDistinctKeyValuePairs))
            //{
            //    return charSet;
            //}
            //CharSet orderedDistinctCharSet = new CharSet();
            //foreach (KeyValuePair<double, char> keyValuePair in orderedDistinctKeyValuePairs)
            //{
            //    orderedDistinctCharSet.Add(keyValuePair.Key, keyValuePair.Value);
            //}
            //orderedDistinctCharSet.DisplayName = charSet.DisplayName;
            //return orderedDistinctCharSet;

            return charSet;
        }

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

        public void DebugEnumerateKeyValuePairs() // TODO: [HV] Make built assembly bypass all Debug-related methods in release build(s)
        {
            foreach (KeyValuePair<double, char> charInfo in this)
            {
                Debug.WriteLine(charInfo.Key + ", " + charInfo.Value);
            }
        }

        public static bool operator ==(CharSet charSet1, CharSet charSet2)
        {
            if (charSet1 != null && charSet2 != null)
            {
                return charSet1.SequenceEqual(charSet2) && charSet1.DisplayName == charSet2.DisplayName; // [HV] Test for identicality (both identical DisplayName and contents required)
            }
            return false;
        }
        public static bool operator !=(CharSet charSet1, CharSet charSet2) => !(charSet1 == charSet2);

        public override bool Equals(object obj) => Equals(obj as CharSet);
        public bool Equals(CharSet other) => this != null && other != null && this.SequenceEqual(other); // [HV] Test for equivalence (only identical contents required)
        public override int GetHashCode() => base.GetHashCode(); // TODO: [HV] Find a way to make equal (equivalent or identical?) CharSet instances return same hashcodes

        public static void InitializeCharSetFolder()
        {
            if (!Directory.Exists(charSetFolderPath))
            {
                Directory.CreateDirectory(charSetFolderPath);
                ResourceSet resourceSet = Resources.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
                foreach (DictionaryEntry resourceEntry in resourceSet)
                {
                    string resourceName = resourceEntry.Key.ToString();
                    if (resourceName.StartsWith("cs_", StringComparison.InvariantCulture))
                    {
                        using (Stream resourceStream = typeof(CharSet).Assembly.GetManifestResourceStream($"ASCIIsome.Resources.CharSets.{resourceName}.xml"))
                        {
                            using (FileStream fileStream = new FileStream($"{charSetFolderPath}\\{resourceName}.xml", FileMode.Create, FileAccess.Write))
                            {
                                resourceStream.CopyTo(fileStream);
                            }
                        }
                    }
                }
            }
        }
    }
}
