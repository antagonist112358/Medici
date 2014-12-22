using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Medici.Tests
{
    [TestClass]
    public class MaybeMatchTests
    {
        [TestMethod]
        public void TestUnitMatchOnMaybe()
        {
            var shouldResult = Maybe.Try(() => "Hello World");
            var shouldFail = Maybe.Try(() => Int32.Parse("Hello World"));

            shouldResult.Match(c => c
                .Result(s => Assert.AreEqual("Hello World", s))
                .Error(_ => Assert.IsTrue(false))
            );

            shouldFail.Match(c => c
                .Result(i => Assert.IsTrue(false, "String is not a number!"))
                .Error(_ => Assert.IsTrue(true))
            );
        }
    }
}
