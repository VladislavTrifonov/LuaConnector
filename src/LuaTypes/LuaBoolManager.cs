using System;
using System.Collections.Generic;
using System.Text;

namespace LuaConnector.LuaTypes
{
    static class LuaBoolManager
    {
        private static LuaBoolean _luaTrue;
        private static LuaBoolean _luaFalse; 

        public static LuaBoolean GetTrueValue()
        {
            if (_luaTrue == null)
            {
                _luaTrue = new LuaBoolean(true);
            }
            return _luaTrue;
        }

        public static LuaBoolean GetFalseValue()
        {
            if (_luaFalse == null)
            {
                _luaFalse = new LuaBoolean(false);
            }
            return _luaFalse;
        }
    }
}
