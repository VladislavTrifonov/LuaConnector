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
                L.ProcessString("table = {[0] = 1, [1] = {[false] = true}}");
            }
            catch (Exception)
            { 
                Assert.Fail("Something went wrong");
            }
        }

        [TestMethod]
        public void ReadingTable()
        {
            var table = L["table"] as LuaTable;
            if (table == null)
                Assert.Fail("Table is null");
            Assert.AreEqual((long)table[0L], 1L);
            var nestedTable = table[1L] as LuaTable;
            Assert.AreEqual(nestedTable[false], true);
        }

    }
}
