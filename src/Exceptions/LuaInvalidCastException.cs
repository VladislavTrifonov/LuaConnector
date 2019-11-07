using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LuaConnector.Exceptions
{
    class LuaInvalidCastException : LuaException
    {
        public LuaInvalidCastException()
        {
        }

        public LuaInvalidCastException(string message) : base(message)
        {
        }

        public LuaInvalidCastException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LuaInvalidCastException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
