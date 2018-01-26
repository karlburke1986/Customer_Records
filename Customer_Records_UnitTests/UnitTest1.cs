using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Customer_Records;

namespace Customer_Records_UnitTests
{
    [TestClass]
    public class CustomerRecordsTests
    {
        [TestMethod]
        public void Coordinates_Outside_Of_Tolerence()
        {
            var result = Program.FindDistance(38.8977, 77.0365);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Coordinates_Inside_Of_Tolerence()
        {
            var result = Program.FindDistance(53.2866, -6.3717);

            Assert.IsTrue(result);
        }                
    }
}
