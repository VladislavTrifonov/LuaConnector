using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LuaConnector.Exceptions
{
    class LuaInvalidArgumentException : LuaException
    {
        public LuaInvalidArgumentException()
        {
        }

        public LuaInvalidArgumentException(string message) : base(message)
        {
        }

        public LuaInvalidArgumentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LuaInvalidArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
