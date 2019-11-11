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
        private LuaTable tbl = new LuaTable();

        [TestInitialize]
        public void SetUp()
        {
            try 
            {
                L.ProcessString(@"table = {
                                    [false] = true, 
                                    [25] = 'string',
                                    ['str'] = 32, 
                                    ['dbl'] = 23.4,
                                    ['nested'] = {
                                        [0] = 1
                                    }
                                }");
                tbl = L["table"] as LuaTable;
            }
            catch (Exception)
            { 
                Assert.Fail("Something went wrong");
            }
        }


        [TestMethod]
        public void ReadLongAndStr() => Assert.AreEqual(tbl[25L], "string");

        [TestMethod]
        public void ReadBoolAndBool() => Assert.AreEqual(tbl[false], true);

        [TestMethod]
        public void ReadStrAndLong() => Assert.AreEqual(tbl["str"], 32L);

        [TestMethod]
        public void ReadStrAndDouble() => Assert.AreEqual(tbl["dbl"], (double)23.4);

        [TestMethod]
        public void ReadNested()
        {
            var nestedTbl = tbl["nested"] as LuaTable;
            Assert.AreEqual(nestedTbl[0L], 1L);
        }
        
        [TestMethod]
        public void AddKVInExistTable()
        {
            var newTable = new LuaTable();
            newTable.Add("key", "value");
            newTable.Add("isTrue", false);
            L["table"] = newTable;
            Assert.AreEqual((L["table"] as LuaTable)["key"], "value");
            Assert.AreEqual((L["table"] as LuaTable)["isTrue"], false);
        }

        [TestMethod]
        public void AddNewTable()
        {
            var newTable = new LuaTable();
            newTable.Add("keys", "test");
            newTable.Add("isTrue", false);
            newTable.Add("num", 325);
            L["newtbl"] = newTable;
            var rTbl = L["newtbl"] as LuaTable; // Readed Table (rTbl)
            Assert.AreEqual(rTbl["keys"], "test");
            Assert.AreEqual(rTbl["isTrue"], false);
            Assert.AreEqual(rTbl["num"], 325L);
        }
        
        [TestMethod]
        public void AddNestedTable()
        {
            var newTable = new LuaTable();
            var nestedTable = new LuaTable();
            nestedTable.Add(23, 32);
            nestedTable.Add(43, 34);
            newTable.Add("numbers", nestedTable);
            newTable.Add("key", "value");
            L["nTbl"] = newTable;
            var rTbl = L["nTbl"] as LuaTable;
            Assert.AreEqual(rTbl["key"], "value");
            Assert.AreEqual((rTbl["numbers"] as LuaTable)[23L], 32L);
            Assert.AreEqual((rTbl["numbers"] as LuaTable)[43L], 34L);
        }
    }
}
