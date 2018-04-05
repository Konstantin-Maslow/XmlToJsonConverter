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
            converter.AddRule(new Rule(IsCapability, XmlToJsonConvertFirstChild));
            converter.AddRule(new Rule(IsLayer, XmlToJsonLayer));
            XmlNode xmlRootNode = xmlDocument.FirstChild;
            JsonObject jsonObject = new JsonObject();
            converter.Convert(xmlRootNode, jsonObject);

            File.WriteAllText(jsonPath, jsonObject.GetAsString());
        }

        static private bool IsCapability(XmlNode xmlNode)
        {
            return xmlNode.Name == "Capability";
        }

        static private bool IsLayer(XmlNode xmlNode)
        {
            return xmlNode.Name == "Layer";
        }

        static private void XmlToJsonConvertFirstChild(XmlNode xmlNode, JsonNode jsonNode, XmlToJsonConverter converter)
        {
            converter.Convert(xmlNode.FirstChild, jsonNode);
        }

        static private void XmlToJsonLayer(XmlNode xmlNode, JsonNode jsonNode, XmlToJsonConverter converter)
        {
            XmlNode nameXmlNode = xmlNode.SelectSingleNode("Name");
            XmlNode titleXmlNode = xmlNode.SelectSingleNode("Title");

            string name = nameXmlNode.InnerText;
            string title = "";
            if(titleXmlNode != null)
            {
                title = titleXmlNode.InnerText;
            }

            jsonNode.Add("name", new JsonValue(name));
            jsonNode.Add("title", new JsonValue(title));

            XmlNodeList sublayersXmlNodes = xmlNode.SelectNodes("Layer");
            if (sublayersXmlNodes.Count > 0)
            {
                XmlToJsonSublayers(sublayersXmlNodes, jsonNode, converter);
            }

            XmlNode attributesXmlNode = xmlNode.SelectSingleNode("Attributes");
            if(attributesXmlNode != null)
            {
                XmlToJsonAttributes(attributesXmlNode, jsonNode);
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

        static private void XmlToJsonAttributes(XmlNode xmlNode, JsonNode jsonNode)
        {
            JsonList layerAttributes = new JsonList();
            foreach(XmlNode attributeXmlNode in xmlNode.ChildNodes)
            {
                JsonObject layerAttribute = new JsonObject();
                XmlAttributeCollection xmlAttributes = attributeXmlNode.Attributes;
                string name = xmlAttributes.GetNamedItem("name").InnerText;
                string type = xmlAttributes.GetNamedItem("type").InnerText;
                layerAttribute.Add("name", new JsonValue(name));
                layerAttribute.Add("type", new JsonValue(type));
                layerAttributes.Add(layerAttribute);
            }
            jsonNode.Add("attributes", layerAttributes);
        }
    }
}
