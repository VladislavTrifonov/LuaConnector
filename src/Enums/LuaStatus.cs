using System;
using System.Collections.Generic;
using System.Text;

namespace LuaConnector.Enums
{
    public enum LuaStatus
    {
        Ok = 0x1, 
        Broken = 0x2, 
        ReadyToRun = 0x4,
    }
}
