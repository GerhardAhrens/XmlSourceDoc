
using EasyPrototyping.XMLSourceDoc;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EasyPrototypingTest
{
    [TestClass]
    public class XmlSourceDocumentation_Test
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
        }

        [TestMethod]
        public void SplitTerm()
        {
            string input = "(System.Tuple{System.String,System.Boolean},System.Collections.Generic.Dictionary{System.Guid,System.String},System.DateTime,System.String)";

            XmlSourceDocumentation xd = new XmlSourceDocumentation();
            IEnumerable<string> terms = xd.SplitTerm(input);
            Assert.IsNotNull(terms);
            CollectionAssert.AllItemsAreNotNull(terms.ToList());
        }
    }
}
