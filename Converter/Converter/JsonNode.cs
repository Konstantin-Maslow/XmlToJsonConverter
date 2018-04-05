using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    interface JsonNode
    {
        void Add(JsonNode jsonNode);
        void Add(string key, JsonNode jsonNode);
        string GetAsString(int depth = 0, bool previousIsKey = true);
    }
}
