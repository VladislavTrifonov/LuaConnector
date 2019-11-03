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

        private bool disposed = false;
        private readonly IntPtr lua_State;

        private List<string> files = new List<string>();

        

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

        public (int, bool) ProcessFile(string fileName)
        {
            var ret = CApi.luaL_loadfilex(lua_State, fileName, null);
            //var ret_2 = CApi.lua_pcallk(lua_State, 0, -1, 0, IntPtr.Zero, IntPtr.Zero);
            /*  if (ret == LuaError.Memory)
              {
                  Status = LuaStatus.Broken;
                  throw new LuaOutOfMemoryException($"Memory is out while processing file {fileName}");
              }
              else if (ret == LuaError.Syntax)
              {
                  var cmessage = CApi.luaL_tolstring(lua_State, -1, out UIntPtr size);
                  var message = Marshal.PtrToStringAnsi(cmessage, (int)size);
                  throw new LuaSyntaxException(message);
              }
              else
              {*/
            var cmessage = CApi.luaL_tolstring(lua_State, -1, out UIntPtr size);
            var message = Marshal.PtrToStringAnsi(cmessage, (int)size);
            throw new LuaSyntaxException(message);
            
            //}
                
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
