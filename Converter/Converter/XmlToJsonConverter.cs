using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Converter
{
    class XmlToJsonConverter
    {
        public XmlToJsonConverter()
        {
            _Rules = new List<Rule>();
        }

        public void AddRule(Rule rule)
        {
            _Rules.Add(rule);
        }

        public JsonNode Convert(XmlNode xmlNode, JsonNode jsonNode)
        {
            foreach(Rule rule in _Rules)
            {
                if(rule.IsMatching(xmlNode))
                {
                    rule.Apply(xmlNode, jsonNode, this);
                    break;
                }
            }
            return jsonNode;
        }

        private List<Rule> _Rules;
    }
}
