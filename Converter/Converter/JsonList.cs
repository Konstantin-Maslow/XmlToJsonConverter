using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    class JsonList : JsonNode
    {
        public JsonList()
        {
            _InnerNodes = new List<JsonNode>();
        }

        public void Add(JsonNode jsonNode)
        {
            _InnerNodes.Add(jsonNode);
        }

        public void Add(string key, JsonNode jsonNode)
        {
            throw new JsonNodeException("Невозможна вставка значения с ключом в JsonList");
        }

        public string GetAsString(int depth = 0, bool previousIsKey = true)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string indent = new string('\t', depth);
            stringBuilder.Append(string.Format("{0}[\n", previousIsKey ? "" : indent));

            for (int innerNodeIndex = 0; innerNodeIndex < _InnerNodes.Count; ++innerNodeIndex)
            {
                JsonNode innerNode = _InnerNodes[innerNodeIndex];
                stringBuilder.Append(innerNode.GetAsString(depth + 1, false));
                if(innerNodeIndex != _InnerNodes.Count - 1)
                {
                    stringBuilder.Append(",\n");
                }
            }

            stringBuilder.Append(string.Format("\n{0}]", indent));
            return stringBuilder.ToString();
        }

        private List<JsonNode> _InnerNodes;
    }
}
