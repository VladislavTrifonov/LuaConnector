using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using LuaConnector.Exceptions;
using LuaConnector.Enums;
using System.Diagnostics;

namespace LuaConnector
{
    public class LuaState : IDisposable
    {

        public LuaStatus Status { get; private set; } = LuaStatus.ReadyToRun;

        public object this[string name]
        {
            get
            {
                CApi.lua_getglobal(lua_State, name);
                return LuaObjToCLRObj(-1);
            }
            set
            {
                PushCLRObj(value, name);
                CApi.lua_setglobal(lua_State, name);
            }
        }

        private const int LUA_MULTIRET = -1;

        private bool disposed = false;
     
        private readonly IntPtr lua_State;     

        public LuaState(bool loadLibs = true)
        {
            lua_State = CApi.luaL_newstate();

            if (loadLibs)
                CApi.luaL_openlibs(lua_State);

            Status = LuaStatus.Ok;
        }

        public void ProcessFilesInDir(string path)
        {
            var files = Directory.GetFiles(path, "*.lua");
            foreach (var file in files)
                ProcessFile(file);

            var subDirs = Directory.GetDirectories(path);
            foreach (var subDir in subDirs)
                ProcessFilesInDir(subDir);
        }

        public void ProcessFile(string fileName)
        {
            LuaError ret;
            ret = (LuaError)CApi.luaL_loadfilex(lua_State, fileName, null);
            if (ret != LuaError.OK)
                ErrorCodeToException(ret);

            ret = (LuaError)CApi.lua_pcallk(lua_State, 0, LUA_MULTIRET, 0, IntPtr.Zero, IntPtr.Zero);
            if (ret != LuaError.OK)
                ErrorCodeToException(ret);

        }

        public void ProcessString(string str)
        {
            CApi.luaL_loadbufferx(lua_State, Encoding.ASCII.GetBytes(str), (UIntPtr)str.Length, "ProcessStringChunk", null);

            LuaError ret;
            ret = (LuaError)CApi.lua_pcallk(lua_State, 0, LUA_MULTIRET, 0, IntPtr.Zero, IntPtr.Zero);
            if (ret != LuaError.OK)
                ErrorCodeToException(ret);
            
        }

        private void PushCLRObj(object obj, string name = "")
        {
            switch (obj)
            {
                case bool b:
                    CApi.lua_pushboolean(lua_State, Convert.ToInt32(b));
                    break;
                case double d:
                    CApi.lua_pushnumber(lua_State, d);
                    break;
                case int i:
                    CApi.lua_pushinteger(lua_State, i);
                    break;
                case long l:
                    CApi.lua_pushinteger(lua_State, l);
                    break;
                case string str:
                    {
                        byte[] s = Encoding.ASCII.GetBytes(str);
                        CApi.lua_pushlstring(lua_State, s, (UIntPtr)str.Length);
                        break;
                    }
                case LuaTable tbl:
                    {
                        foreach (var kv in tbl)
                        {
                            if (CApi.lua_type(lua_State, -1) != (int)LuaTypes.Table)
                            {
                                CApi.lua_getglobal(lua_State, name);

                                if (CApi.lua_type(lua_State, -1) != (int)LuaTypes.Table)
                                {
                                    CApi.lua_settop(lua_State, -2);
                                    CApi.lua_createtable(lua_State, 0, 0);
                                    
                                }
                            }
                            PushCLRObj(kv.Key);
                            PushCLRObj(kv.Value);
                            
                            CApi.lua_settable(lua_State, -3);
                           
                        }
                        break;
                    }
                case null:
                    CApi.lua_pushnil(lua_State);
                    break;
                default:
                    throw new LuaException("Unsupported type for push object!");
            }
        }
        
        private object LuaObjToCLRObj(int index)
        {
            switch ((LuaTypes)CApi.lua_type(lua_State, index))
            {
                case LuaTypes.Nil:
                    return null;

                case LuaTypes.Boolean:
                    return ConvertToBool(index);

                case LuaTypes.String:
                    return ConvertToString(index);

                case LuaTypes.Number:
                    {
                        if (CApi.lua_isinteger(lua_State, index) == 0)
                            return ConvertToDouble(index);
                        else
                            return ConvertToInt(index);
                    }
                case LuaTypes.Table:
                    {
                        return ConvertToTable(index);
                    }
                default:
                    throw new LuaException($"Unsupported type of index {index} ({Marshal.PtrToStringAnsi(CApi.lua_typename(lua_State, CApi.lua_type(lua_State, index)))})");
            }
        }

        private void ErrorCodeToException(LuaError returnCode)
        {
            switch (returnCode)
            {
                case LuaError.FileNotFound:
                    throw new LuaProcessFileException(ConvertToString(-1));

                case LuaError.GCMetaMethod:
                case LuaError.Runtime:
                case LuaError.Unknown:
                    throw new LuaException($"Something went wrong, LuaError returns {returnCode}");

                case LuaError.Memory:
                    throw new LuaOutOfMemoryException("Memory is over");

                case LuaError.Syntax:
                    throw new LuaSyntaxException(ConvertToString(-1));
            }
        }

        private string ConvertToString(int index, bool callMM = false)
        {
            IntPtr c_message;
            UIntPtr size;
            if (callMM)
            {
                c_message = CApi.luaL_tolstring(lua_State, index, out size); // gets C-string from Lua-stack
                CApi.lua_settop(lua_State, -2);
            }
            else
                c_message = CApi.lua_tolstring(lua_State, index, out size);

            return Marshal.PtrToStringAnsi(c_message, (int)size);
        }

        private bool ConvertToBool(int index)
        {
            if ((LuaTypes)CApi.lua_type(lua_State, index) != LuaTypes.Boolean)
                throw new LuaInvalidCastException($"Index {index} doesn't contain a boolean value.");

            return CApi.lua_toboolean(lua_State, index) == 1;
        }

        private double ConvertToDouble(int index)
        {
            if (CApi.lua_isnumber(lua_State, index) == 0)
                throw new LuaInvalidCastException($"Index {index} doesn't contain a double value.");
            
            var convertedValue = CApi.lua_tonumberx(lua_State, index, out int isNum);
            if (isNum == 0)
                throw new LuaInvalidCastException($"Failed to convert index {index} to double. Index contains {ConvertToString(index)}");
            return convertedValue;
        }

        private long ConvertToInt(int index)
        {
            if (CApi.lua_isinteger(lua_State, index) == 0)
                throw new LuaInvalidCastException($"Index {index} doesn't contain a integer value");

            var convertedValue = CApi.lua_tointegerx(lua_State, index, out int isNum);
            if (isNum == 0)
                throw new LuaInvalidCastException($"Failed to convert index {index} to double. Index contains {ConvertToString(index)}");
            return convertedValue;
        }
        private bool IsString(int index) => CApi.lua_type(lua_State, index) == (int)LuaTypes.String;
        private bool IsBoolean(int index) => CApi.lua_type(lua_State, index) == (int)LuaTypes.Boolean;
        private bool IsNil(int index) => CApi.lua_type(lua_State, index) == (int)LuaTypes.Nil;
        private bool IsTable(int index) => CApi.lua_type(lua_State, index) == (int)LuaTypes.Table;
        private bool IsNumber(int index) => CApi.lua_isnumber(lua_State, index) == 1;
        private bool IsInteger(int index) => CApi.lua_isinteger(lua_State, index) == 1;

        private LuaTable ConvertToTable(int index)
        {
            var table = new LuaTable();
            var tempIndex = index < 0 ? index - 1 : index;
            CApi.lua_pushnil(lua_State);
            
            while (CApi.lua_next(lua_State, tempIndex) != 0)
            {
                try
                {          
                    table.Add(LuaObjToCLRObj(-2), LuaObjToCLRObj(-1));
                }
                catch (LuaInvalidArgumentException)
                { }
                
                CApi.lua_settop(lua_State, -2);
            }
           
            return table;
        }


        ~LuaState()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            disposed = true;

            if (disposing)
            {
                // releasing managed resources
            }

            // unmanaged resources
            if (lua_State != IntPtr.Zero)
                CApi.lua_close(lua_State);
        }

        private void OnPanic(IntPtr state)
        {
            
        }
    }

 
}
