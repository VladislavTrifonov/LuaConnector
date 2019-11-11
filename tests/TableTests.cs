using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LuaConnector;
using LuaConnector.Exceptions;

namespace tests
{
    [TestClass]
    public class TableTests
    {
        private LuaState L = new LuaState();

        [TestInitialize]
        public void SetUp()
        {
            try 
            {
                L.ProcessString("countries = {['Kt'] = {['Tk'] = 25}}");
            }
            catch (Exception)
            { 
                Assert.Fail("Something went wrong");
            }
        }

        [TestMethod]
        public void ReadingTable()
        {
            var table = L["countries"] as LuaTable;
            if (table == null)
                Assert.Fail("Table is null");
            var nestedTable = table["Kt"] as LuaTable;
            Assert.AreEqual(nestedTable["Tk"], 25L);
            
        }

    }
}
