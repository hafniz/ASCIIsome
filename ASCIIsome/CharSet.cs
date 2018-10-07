using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ASCIIsome
{
    public class CharSet : Dictionary<double, char>
    {
        public string DisplayName { get; set; }

        public static CharSet Concat(params CharSet[] charSets)
        {
            CharSet charSetsJoined = new CharSet();
            foreach (CharSet charSet in charSets)
            {
                foreach (KeyValuePair<double, char> keyValuePair in charSet)
                {
                    charSetsJoined.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            return OrderedDistinct(charSetsJoined);
        }
        public static CharSet operator &(CharSet charSet1, CharSet charSet2) => Concat(charSet1, charSet2);

        private static CharSet OrderedDistinct(CharSet charSet)
        {
            IEnumerable<KeyValuePair<double, char>> orderedDistinctKeyValuePairs = charSet.Distinct().OrderBy(x => x.Key);
            CharSet orderedDistinctCharSet = new CharSet();
            foreach (KeyValuePair<double, char> keyValuePair in orderedDistinctKeyValuePairs)
            {
                orderedDistinctCharSet.Add(keyValuePair.Key, keyValuePair.Value);
            }
            orderedDistinctCharSet.DisplayName = charSet.DisplayName;
            return orderedDistinctCharSet;
        }

        public static CharSet ParseFromXMLFile(string filePath) // Validation/Exception handling needed
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
            return OrderedDistinct(parsedCharSet);
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
                CharSet orderedDistinctCharSet = OrderedDistinct(this);
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

        public void DebugEnumerateKeyValuePairs()
        {
            foreach (KeyValuePair<double, char> keyValuePair in this)
            {
                Debug.WriteLine(keyValuePair.Key + ", " + keyValuePair.Value);
            }
        }
    }
}
