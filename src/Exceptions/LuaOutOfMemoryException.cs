using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LuaConnector.Exceptions
{
    public class LuaOutOfMemoryException : LuaException
    {
        public LuaOutOfMemoryException()
        {
        }

        public LuaOutOfMemoryException(string message) : base(message)
        {
        }

        public LuaOutOfMemoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LuaOutOfMemoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
