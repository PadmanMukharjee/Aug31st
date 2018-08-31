using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.SqlClient;
using M3Pact.LoggerUtility;

namespace LoggerUtility.Parser
{
    /// <summary>
    /// For Interpreting XML file
    /// </summary>
    class XMLParser
    {
        private readonly XmlDocument _xmlDoc;
        /// <summary>
        /// Initialize instance variabel and load xml document
        /// </summary>
        /// <param name="xmlFilePath">Path of a XML File which need to be parsed</param>
        public XMLParser(string xmlFilePath)
        {
            _xmlDoc = new XmlDocument();
            _xmlDoc.LoadXml(File.ReadAllText(xmlFilePath));
        }

        /// <summary>
        /// Creates a list of attribute values of all the child nodes of a passed parent node
        /// </summary>
        /// <param name="parentNodeName">Parent Node whose children attribute value need to be extractd</param>
        /// <param name="childNodeAttributeName">Attribute of the children node for which list need to be created</param>
        /// <returns>List of attribute values of all the child element of the passed parent element</returns>
        public List<string> GetNodeNames(string parentNode, string childNodeAttributeName)
        {
            XmlNodeList NodeList = _xmlDoc.GetElementsByTagName(parentNode);
            var childNodeAttribute = new List<string>();
            foreach (XmlNode node in NodeList)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    childNodeAttribute.Add(GetChildAttributeValue(childNode, childNodeAttributeName));
                }
            }
            return childNodeAttribute;
        }

        /// <summary>
        /// Extract single node attribute value in  XML
        /// </summary>
        /// <param name="attributeName">Attribute of a node for which value need to be extracted</param>
        /// <returns>Attribute value of a single node</returns>
        public string GetSingleNodeAttributeValue(string attributeName)
        {
            string attribute = _xmlDoc.SelectSingleNode(attributeName)?.Value;
            return attribute;
        }

        /// <summary>
        /// Extract the inner text value of a single node in XML
        /// </summary>
        /// <param name="nodeName">Single Node for which inner text value need to be extracted</param>
        /// <returns>Inner text value of a passed node</returns>
        public string GetSingleNodeInnerText(string nodeName)
        {
            string nodeText = _xmlDoc.SelectSingleNode(nodeName)?.InnerText;
            return nodeText;
        }

        /// <summary>
        /// Extracts the attribute value of a child node
        /// </summary>
        /// <param name="childNode">Child Node for whose attribute value need to be extracted</param>
        /// <param name="attributeName">Attribute for which value need to be extracted</param>
        /// <returns>Attribute Value of a passed attribute of a passed child node</returns>
        private string GetChildAttributeValue(XmlNode childNode, string attributeName)
        {
            string childAttribute = childNode.Attributes[attributeName]?.Value;
            return childAttribute;
        }

        /// <summary>
        /// Generate a list with the combined value of two attributes of the child nodes for the passed parent node.
        /// The combined string has two attribute value separated by a space
        /// </summary>
        /// <param name="parentNode">Parent Node whose children attribute value need to be combined</param>
        /// <param name="firstAttributeNameOfChildNode">first attribute of the child element whose value need to be combine</param>
        /// <param name="secondAttributeNameOfChildNode">second attribute of the child element whose value need to be combine </param>
        /// <returns>List of combined value of two attributes of all the child nodes </returns>
        public List<string> GenerateListWithTwoAttributesOfChildNodeCombine(string parentNode, string firstAttributeNameOfChildNode, string secondAttributeNameOfChildNode)
        {
            XmlNodeList ColumnNodeList = _xmlDoc.GetElementsByTagName(parentNode);
            List<string> twoAttributeCombinedList = new List<string>();
            foreach (XmlNode node in ColumnNodeList)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    twoAttributeCombinedList.Add(string.Join(" ", GetChildAttributeValue(childNode, firstAttributeNameOfChildNode), GetChildAttributeValue(childNode, secondAttributeNameOfChildNode)));
                }
            }
            return twoAttributeCombinedList;
        }

        /// <summary>
        /// It repeats the same action for each child node
        /// </summary>
        /// <param name="message">Message string which need to be passed in repeated action</param>
        /// <param name="exception">Exception object which need to be passed in repeated action</param>
        /// <param name="command">Command Objet which need to be passed in repeated action</param>
        /// <param name="parentNode">Parent Node for whose child nodes action need to be repeated</param>
        /// <param name="addParameterForMappedColumn">Action Delegate for the action which need to be repeated for each child node</param>
        public void RepeatActionForEachChildNode(string message, Exception exception, SqlCommand command, string parentNode, Action<string, Exception, SqlCommand, string, string> addParameterForMappedColumn)
        {
            XmlNodeList NodeList = _xmlDoc.GetElementsByTagName(parentNode);
            foreach (XmlNode node in NodeList)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    addParameterForMappedColumn(message, exception, command, GetChildAttributeValue(childNode, Constants.DBConfig_Attribute_MappedTo).ToLower(), GetChildAttributeValue(childNode, Constants.DBConfig_Attribute_Name));
                }
            }
        }
    }
}
