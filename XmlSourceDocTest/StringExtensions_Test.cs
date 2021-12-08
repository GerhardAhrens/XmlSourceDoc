
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyPrototypingTest
{
    [TestClass]
    public class StringExtensions_Test
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
        }

        [TestMethod]
        public void SplitAt()
        {
            string input = "Test-A,Test-B,Test-C";
            int[] posIndex = new int[] {7, 14};
            IEnumerable<string> result = input.SplitAt(',',posIndex);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 3);
        }

    }
}
