using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LuaConnector.Exceptions
{
    public class LuaSyntaxException : LuaException
    {
        public LuaSyntaxException()
        {
        }

        public LuaSyntaxException(string message) : base(message)
        {
        }

        public LuaSyntaxException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LuaSyntaxException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
