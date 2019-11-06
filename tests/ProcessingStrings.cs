using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LuaConnector;
using LuaConnector.Exceptions;

namespace tests
{
    [TestClass]
    public class ProcessingStrings
    {
        [TestMethod]
        public void ProcessingString()
        {

            using (var Lua = new LuaState())
            {
                try
                {
                    Lua.ProcessString("return 2");
                }
                catch (LuaException ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

    }
}
