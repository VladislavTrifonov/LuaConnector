using System;
using System.Collections.Generic;
using System.Text;

namespace LuaConnector
{
    public class LuaNumber : LuaObject
    {
        public double Value { get; }

        public int IntValue { get; }

        public LuaNumber() { }

        public LuaNumber(double value)
            => (Value, IntValue) = (value, Convert.ToInt32(value));

        public LuaNumber(int value)
            => (Value, IntValue) = (value, value);

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
