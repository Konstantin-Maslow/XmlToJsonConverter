using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 2)
            {
                Console.WriteLine("ERROR: Пожалуйста, укажите два параметра командной строки: путь до XML-файла и путь для создания JSON-файла");
                return;
            }

            string xmlPath = args[0];
            string jsonPath = args[1];

            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(xmlPath);
            }
            catch (Exception exception)
            {
                Console.WriteLine(string.Format("ERROR: Возникла ошибка при чтении XML-файла: {0}", exception.Message));
                return;
            }

            
            XmlToJsonConverter converter = new XmlToJsonConverter();
            converter.AddRule(new Rule(IsDocument, XmlToJsonConvertFirstChild));
            converter.AddRule(new Rule(IsCapability, XmlToJsonConvertFirstChild));
            converter.AddRule(new Rule(IsLayer, XmlToJsonLayer));
            converter.AddRule(new Rule(IsAttributes, XmlToJsonAttributes));
            converter.AddRule(new Rule(IsAttribute, XmlToJsonAttribute));

            JsonObject jsonObject = new JsonObject();
            converter.Convert(xmlDocument, jsonObject);
            
            File.WriteAllText(jsonPath, jsonObject.GetAsString());
        }

        static private bool IsDocument(XmlNode xmlNode)
        {
            return xmlNode.Name == "#document";
        }

        static private bool IsCapability(XmlNode xmlNode)
        {
            return xmlNode.Name == "Capability";
        }

        static private bool IsLayer(XmlNode xmlNode)
        {
            return xmlNode.Name == "Layer";
        }

        static private bool IsAttributes(XmlNode xmlNode)
        {
            return xmlNode.Name == "Attributes";
        }

        static private bool IsAttribute(XmlNode xmlNode)
        {
            return xmlNode.Name == "Attribute";
        }

        static private void XmlToJsonConvertFirstChild(XmlNode xmlNode, JsonNode jsonNode, XmlToJsonConverter converter)
        {
            converter.Convert(xmlNode.FirstChild, jsonNode);
        }

        static private void XmlToJsonLayer(XmlNode layerNode, JsonNode jsonNode, XmlToJsonConverter converter)
        {
            XmlNode nameXmlNode = layerNode.SelectSingleNode("Name");
            XmlNode titleXmlNode = layerNode.SelectSingleNode("Title");

            string name = nameXmlNode.InnerText;
            string title = "";
            if(titleXmlNode != null)
            {
                title = titleXmlNode.InnerText;
            }

            jsonNode.Add("name", new JsonValue(name));
            jsonNode.Add("title", new JsonValue(title));

            XmlNodeList sublayersXmlNodes = layerNode.SelectNodes("Layer");
            if (sublayersXmlNodes.Count > 0)
            {
                XmlToJsonSublayers(sublayersXmlNodes, jsonNode, converter);
            }

            XmlNode attributesXmlNode = layerNode.SelectSingleNode("Attributes");
            if(attributesXmlNode != null)
            {
                JsonList layerAttributes = new JsonList();
                XmlToJsonAttributes(attributesXmlNode, layerAttributes, converter);
                jsonNode.Add("attributes", layerAttributes);
            }
        }

        static private void XmlToJsonSublayers(XmlNodeList sublayersXmlNodes, JsonNode jsonNode, XmlToJsonConverter converter)
        {
            JsonList sublayersJsonNodes = new JsonList();
            foreach (XmlNode sublayerXmlNode in sublayersXmlNodes)
            {
                JsonObject sublayerJsonNode = new JsonObject();
                converter.Convert(sublayerXmlNode, sublayerJsonNode);
                sublayersJsonNodes.Add(sublayerJsonNode);
            }
            jsonNode.Add("sublayers", sublayersJsonNodes);
        }

        static private void XmlToJsonAttributes(XmlNode attributesNode, JsonNode jsonNode, XmlToJsonConverter converter)
        {
            foreach(XmlNode attributeXmlNode in attributesNode.ChildNodes)
            {
                JsonObject layerAttribute = new JsonObject();
                converter.Convert(attributeXmlNode, layerAttribute);
                jsonNode.Add(layerAttribute);
            }
        }

        static private void XmlToJsonAttribute(XmlNode attributeNode, JsonNode jsonNode, XmlToJsonConverter converter)
        {
            XmlAttributeCollection xmlAttributes = attributeNode.Attributes;
            string name = xmlAttributes.GetNamedItem("name").InnerText;
            string type = xmlAttributes.GetNamedItem("type").InnerText;
            jsonNode.Add("name", new JsonValue(name));
            jsonNode.Add("type", new JsonValue(type));
        }
    }
}
