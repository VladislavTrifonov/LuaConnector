using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using LuaConnector.LuaTypes;
using LuaConnector.Exceptions;
using LuaConnector.Enums;
using System.Runtime.InteropServices;

namespace LuaConnector
{
    public class LuaState : IDisposable
    {
        public IntPtr Handle => lua_State;

        public LuaStatus Status { get; private set; } = LuaStatus.ReadyToRun;

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

        private void ErrorCodeToException(LuaError returnCode)
        {
            switch (returnCode)
            {
                case LuaError.FileNotFound:
                    throw new LuaProcessFileException(GetString(-1));

                case LuaError.GCMetaMethod:
                case LuaError.Runtime:
                case LuaError.Unknown:
                    throw new LuaException($"Something went wrong, LuaError returns {returnCode}");

                case LuaError.Memory:
                    throw new LuaOutOfMemoryException("Memory is over");

                case LuaError.Syntax:
                    throw new LuaSyntaxException(GetString(-1));
            }
        }

        private string GetString(int index)
        {
            var c_message = CApi.luaL_tolstring(lua_State, index, out UIntPtr size); // gets C-string from Lua-stack
            return Marshal.PtrToStringAnsi(c_message, (int)size);
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
