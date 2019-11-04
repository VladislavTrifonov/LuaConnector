using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LuaConnector.Exceptions
{
    public class LuaProcessFileException : LuaException
    {
        public LuaProcessFileException()
        {
        }

        public LuaProcessFileException(string message) : base(message)
        {
        }

        public LuaProcessFileException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LuaProcessFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
