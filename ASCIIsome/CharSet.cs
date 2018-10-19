using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace ASCIIsome
{
    public class CharSet : Dictionary<double, char>, IEquatable<CharSet>
    {
        private static readonly double minimalGrayscaleDivision = 1E-5;
        private static readonly Range<double> defaultGrayscaleRange = new Range<double>(0.00000, 1.00000);
        public string DisplayName { get; set; }
        public override string ToString() => DisplayName;

        public static CharSet Concat(params CharSet[] charSets)
        {
            CharSet charSetsJoined = new CharSet { DisplayName = $"{charSets.Count()} charsets selected" };
            foreach (CharSet charSet in charSets)
            {
                foreach (KeyValuePair<double, char> keyValuePair in charSet)
                {
                    charSetsJoined.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            return MakeOrderedDistinct(charSetsJoined);
        }
        public static CharSet operator |(CharSet charSet1, CharSet charSet2) => Concat(charSet1, charSet2);

        private static CharSet MakeOrderedDistinct(CharSet charSet)
        {
            IEnumerable<KeyValuePair<double, char>> orderedDistinctKeyValuePairs = charSet.Distinct().OrderBy(x => x.Key);
            if (charSet.SequenceEqual(orderedDistinctKeyValuePairs))
            {
                return charSet;
            }
            else
            {
                CharSet orderedDistinctCharSet = new CharSet();
                foreach (KeyValuePair<double, char> keyValuePair in orderedDistinctKeyValuePairs)
                {
                    orderedDistinctCharSet.Add(keyValuePair.Key, keyValuePair.Value);
                }
                orderedDistinctCharSet.DisplayName = charSet.DisplayName;
                return orderedDistinctCharSet;
            }
        }

        private new void Add(double key, char value)
        {
            if (ContainsValue(value))
            {
                return;
            }
            while (key <= defaultGrayscaleRange.Maximum)
            {
                try
                {
                    base.Add(key, value);
                    break;
                }
                catch (ArgumentException)
                {
                    key += minimalGrayscaleDivision; // WARNING: [HV] This particular approach can theoretically cause significant difference between the actual value of grayscale index of a character and the value assigned when being added into a charset,
                                                     // in case that all neighboring keys have been occupied. OR, if all possible values of key between it's original grayscale index and DefaultGrayscaleRange.Maximum have already been occupied, 
                                                     // the character will be discarded instead of being added to the charset, which may be unexpected to the end user
                }
            }
        }
        
        public static CharSet ParseFromXMLFile(string filePath) // TODO: [HV] Validation/Exception handling needed (in external code)
        {
            CharSet parsedCharSet = new CharSet();
            XmlDocument document = new XmlDocument();
            document.Load(filePath);
            XmlNode rootNode = document.DocumentElement as XmlNode;
            XmlNodeList nodeList = rootNode.ChildNodes;
            foreach (XmlNode keyValuePairNode in nodeList)
            {
                double parsedGrayscaleIndex = double.Parse(keyValuePairNode.Attributes["GrayscaleIndex"].Value);
                char parsedCharacter = char.Parse(keyValuePairNode.Attributes["Character"].Value);
                parsedCharSet.Add(parsedGrayscaleIndex, parsedCharacter);
            }
            parsedCharSet.DisplayName = rootNode.Attributes["DisplayName"].Value;
            return MakeOrderedDistinct(parsedCharSet);
        }

        public void ExportToXMLFile(string filePath)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings { Indent = true };
            using (XmlWriter xmlWriter = XmlWriter.Create(filePath, xmlWriterSettings))
            {
                xmlWriter.WriteStartElement("CharSet");
                xmlWriter.WriteAttributeString("DisplayName", DisplayName);
                xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                xmlWriter.WriteAttributeString("xsi", "schemaLocation", null, "ASCIIsome.CharSets CharSetSchema.xsd");
                CharSet orderedDistinctCharSet = MakeOrderedDistinct(this);
                foreach (KeyValuePair<double, char> keyValuePair in orderedDistinctCharSet)
                {
                    xmlWriter.WriteStartElement("KeyValuePair");
                    xmlWriter.WriteAttributeString("GrayscaleIndex", keyValuePair.Key.ToString());
                    xmlWriter.WriteAttributeString("Character", keyValuePair.Value.ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
        }

        public void DebugEnumerateKeyValuePairs() // TODO: [HV] Make built assembly bypass all Debug-related methods in release build(s)
        {
            foreach (KeyValuePair<double, char> keyValuePair in this)
            {
                Debug.WriteLine(keyValuePair.Key + ", " + keyValuePair.Value);
            }
        }

        public static bool operator ==(CharSet charSet1, CharSet charSet2) => charSet1.SequenceEqual(charSet2) && charSet1.DisplayName == charSet2.DisplayName; // [HV] Test for identicality (both identical DisplayName and contents required)
        public static bool operator !=(CharSet charSet1, CharSet charSet2) => !(charSet1 == charSet2);
        public override bool Equals(object obj) => Equals(obj as CharSet);
        public bool Equals(CharSet other) => other != null && this.SequenceEqual(other); // [HV] Test for equivalence (only identical contents required)
        public override int GetHashCode() => 1862586150 + EqualityComparer<string>.Default.GetHashCode(DisplayName); // TODO: [HV] Find a way to make equal (equivalent or identical?) CharSet instances return same hashcodes
    }
}
