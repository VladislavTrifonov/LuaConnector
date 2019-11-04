using Microsoft.VisualStudio.TestTools.UnitTesting;
using LuaConnector;
using LuaConnector.Exceptions;

namespace tests
{
    [TestClass]
    public class ProcessingFiles
    {
        [TestMethod]
        public void ProcessFile()
        {
            var lua = new LuaState();
            try
            {
                lua.ProcessFile("test.lua");
            }
            catch (LuaException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
