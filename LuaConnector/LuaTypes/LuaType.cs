using System;
using System.Collections.Generic;
using System.Text;

namespace LuaConnector
{
    [Flags]
    public enum LuaType
    {
        Nil = 0x0, 
        Number = 0x1, 
        String = 0x2, 
        Boolean = 0x4, 
        Table = 0x8, 
        Userdata = 0x10, 
        Function = 0x20
    }
}
