using System;
using System.Collections.Generic;
using System.Text;
using LuaConnector.LuaTypes;

namespace LuaConnector
{
    public class LuaState : IDisposable
    {
        private bool disposed = false;
        private readonly IntPtr lua_State; 
        public LuaState()
        {

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
                
            }
        }
    }
}
