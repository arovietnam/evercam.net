using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvercamV1
{
    public class EvercamException : Exception
    {
        public EvercamException(): base() { }

        public EvercamException(string message): base(message) { }

        public EvercamException(Exception x) : base(x.Message, x.InnerException) { }

        public EvercamException(string message, Exception innerException): base(message, innerException) { }

        public EvercamException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
