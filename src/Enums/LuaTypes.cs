using System;
using System.Collections.Generic;
using System.Text;

namespace LuaConnector.Enums
{
    public enum LuaTypes
    {
        Nil = 0, 
        Boolean,
        LightUserData,
        Number,
        String,
        Table,
        Function,
        UserData,
        Thread
    }
}
