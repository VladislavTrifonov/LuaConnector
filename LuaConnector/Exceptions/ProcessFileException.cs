using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LuaConnector.Exceptions
{
    public class ProcessFileException : Exception
    {
        public ProcessFileException()
        {
        }

        public ProcessFileException(string message) : base(message)
        {
        }

        public ProcessFileException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProcessFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
