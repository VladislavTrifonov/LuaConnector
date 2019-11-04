using System;
using System.Collections.Generic;
using System.Text;

namespace LuaConnector.Enums
{
    public enum LuaError
    {
        OK = 0x0, 
        Runtime = 0x2,
        Syntax,
        Memory,
        GCMetaMethod,
        Unknown,
        FileNotFound
    }
}
