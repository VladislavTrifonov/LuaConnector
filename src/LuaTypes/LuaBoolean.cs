using System;
using System.Collections.Generic;
using System.Text;


namespace LuaConnector.LuaTypes
{
    public class LuaBoolean : LuaObject
    {
        public bool Value { get; }

        public LuaBoolean() { }

        public LuaBoolean(bool value)
            => Value = value;

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
