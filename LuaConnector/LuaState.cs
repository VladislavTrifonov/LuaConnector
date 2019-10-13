using System;
using System.Collections.Generic;
using System.Text;
using LuaConnector.LuaTypes;
using LuaConnector.Exceptions;

namespace LuaConnector
{
    public class LuaState : IDisposable
    {
        private bool disposed = false;
        private readonly IntPtr lua_State;

        private List<string> files = new List<string>();

        public LuaState(bool loadLibs = true)
        {
            lua_State = CApi.luaL_newstate();

            if (loadLibs)
                CApi.luaL_openlibs(lua_State);
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

        public void ProcessFile(string path, string mode = null)
        {
            bool status = CApi.luaL_loadfilex(lua_State, path, null) == 1;

            if (!status)
                throw new ProcessFileException("Something wrong...");

            files.Add(path);
        }
    }
}
