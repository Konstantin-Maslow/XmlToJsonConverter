using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    class JsonValue : JsonNode
    {
        public JsonValue(object value)
        {
            _Value = value.ToString();
        }

        public void Add(JsonNode jsonNode)
        {
            throw new JsonNodeException("Невозможна вставка значения в JsonValue");
        }

        public void Add(string key, JsonNode jsonNode)
        {
            throw new JsonNodeException("Невозможна вставка значения в JsonValue");
        }

        public string GetAsString(int depth = 0, bool previousIsKey = true)
        {
            return string.Format("\"{0}\"", _Value);
        }

        private string _Value;
    }
}
