using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    class JsonNodeException : Exception
    {
        public JsonNodeException() : base()
        {

        }

        public JsonNodeException(string message) : base(message)
        {

        }

        public JsonNodeException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
