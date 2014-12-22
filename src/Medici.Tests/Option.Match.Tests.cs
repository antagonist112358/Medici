using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Medici.Tests
{
    [TestClass]
    public class OptionMatchTests
    {
        [TestMethod]
        public void TestUnitMatchOnOptions()
        {
            Option<string> j;

            j = Option.Some("Hello World");

            int responseCode = 0;

            j.Match(c => c
                .Some(s => responseCode = 1)
                .None(() => responseCode = 0)
            );

            j = Option.None;

            Assert.IsTrue(responseCode == 1);

            j.Match(c => c
                .Some(s => responseCode = 0)
                .None(() => responseCode = 1)
            );

            Assert.IsTrue(responseCode == 1);
        }

    }
}
