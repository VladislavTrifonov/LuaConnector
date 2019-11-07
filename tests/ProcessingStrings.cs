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
        Random rand = new Random();
        long lRand;
        double dRand;
        string sRand;
        bool bRand;
        [TestInitialize]
        public void SetUp()
        {
            lRand = rand.Next() + int.MaxValue;
            dRand = rand.NextDouble();

            byte[] randomBytes = new byte[20];
            rand.NextBytes(randomBytes);
            sRand = Encoding.ASCII.GetString(randomBytes);

            bRand = rand.Next() % 2 == 0;
        }
        
        [TestMethod]
        public void RandomLong()
        {
            using (var Lua = new LuaState())
            {
                Lua["longVar"] = lRand;
                Assert.AreEqual(Lua["longVar"], lRand);
            }
        }

        [TestMethod]
        public void RandomDouble()
        {
            using (var Lua = new LuaState())
            {
                Lua["doubleVar"] = dRand;
                Assert.AreEqual(Lua["doubleVar"], dRand);
            }
        }

        [TestMethod]
        public void RandomString()
        {
            using (var Lua = new LuaState())
            {
                Lua["strVar"] = sRand;
                Assert.AreEqual(Lua["strVar"], sRand);
            }
        }

        [TestMethod]
        public void RandomBool()
        {
            using (var Lua = new LuaState())
            {
                Lua["boolVar"] = bRand;
                Assert.AreEqual(Lua["boolVar"], bRand);
            }
        }

        [TestMethod]
        public void ProcessingString()
        {

            using (var Lua = new LuaState())
            {
                try
                {
                    Lua.ProcessString("a = 2");
                }
                catch (LuaException ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

    }
}
