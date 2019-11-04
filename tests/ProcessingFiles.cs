using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LuaConnector;
using LuaConnector.Exceptions;

namespace tests
{
    [TestClass]
    public class ProcessingFiles
    {
        [TestInitialize]
        public void Setup()
        {
            var file = File.Open("test.lua", FileMode.OpenOrCreate);
            file.Close();
        }
        [TestMethod]
        public void ProcessFile()
        {
            var lua = new LuaState();
            try
            {
                lua.ProcessFile("test.lua");
            }
            catch (LuaException e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
