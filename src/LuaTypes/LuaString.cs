using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using char_ptr_t = System.IntPtr;

namespace LuaConnector.LuaTypes
{
    public class LuaString : LuaObject
    {
        public string Value { get; }

        public LuaString() { }

        public LuaString(char_ptr_t chars)
        {
            Value = Marshal.PtrToStringAnsi(chars);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
