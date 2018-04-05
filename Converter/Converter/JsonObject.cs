using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    class JsonObject : JsonNode
    {
        public JsonObject()
        {
            _InnerNodes = new Dictionary<string, JsonNode>();
        }

        public void Add(JsonNode jsonNode)
        {
            throw new JsonNodeException("Невозможна вставка значения без ключа в JsonObject");
        }

        public void Add(string key, JsonNode jsonNode)
        {
            _InnerNodes.Add(key, jsonNode);
        }

        public string GetAsString(int depth = 0, bool previousIsKey = true)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string indent = new string('\t', depth);
            stringBuilder.Append(string.Format("{0}{{\n", previousIsKey ? "" : indent));

            for (int innerNodeIndex = 0; innerNodeIndex < _InnerNodes.Count; ++innerNodeIndex)
            {
                KeyValuePair<string, JsonNode> pair = _InnerNodes.ElementAt(innerNodeIndex);
                string key = pair.Key;
                JsonNode innerNode = pair.Value;

                stringBuilder.Append(string.Format("{0}\"{1}\": ", new string('\t', depth + 1), key));
                stringBuilder.Append(innerNode.GetAsString(depth + 1));
                if (innerNodeIndex != _InnerNodes.Count - 1)
                {
                    stringBuilder.Append(",\n");
                }
            }

            stringBuilder.Append(string.Format("\n{0}}}", indent));
            return stringBuilder.ToString();
        }

        private Dictionary<string, JsonNode> _InnerNodes;
    }
}
