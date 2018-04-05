using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Converter
{
    class Rule
    {
        public Rule(Func<XmlNode, bool> matchMethod, Action<XmlNode, JsonNode, XmlToJsonConverter> convertionMethod)
        {
            _MatchFunction = matchMethod;
            _ConvertionAction = convertionMethod;
        }

        public bool IsMatching(XmlNode xmlNode)
        {
            return _MatchFunction(xmlNode);
        }

        public void Apply(XmlNode xmlNode, JsonNode jsonNode, XmlToJsonConverter converter)
        {
            _ConvertionAction(xmlNode, jsonNode, converter);
        }

        private Func<XmlNode, bool> _MatchFunction;
        private Action<XmlNode, JsonNode, XmlToJsonConverter> _ConvertionAction;
    }
}
