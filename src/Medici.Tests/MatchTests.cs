using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Medici.Tests
{
    [TestClass]
    public class MatchTests
    {
        class Base { }

        class Child : Base { }

        class HasProperty
        {
            public int Number { get; set; }
        }

        [TestMethod]
        public void TestUnitMatchOnDerivedTypes()
        {
            Base child = new Child();
            Base @base = new Base();

            child.Match(c => c
                .Case((Child a) => Assert.IsTrue(true))
                .Default(() => Assert.IsTrue(false))
            );

            @base.Match(c => c
                .Case((Child b) => Assert.IsTrue(false))
                .Default(() => Assert.IsTrue(true))
            );

        }

        [TestMethod]
        public void TestValueMatchOnOption()
        {
            var optOfString = Option.Some("Hello");

            int m = optOfString.Match<string, int>(c => c
                .Some(s => s.Length)                
                .None(() => -1)
            );

            Assert.AreEqual(5, m);
        }

        [TestMethod]
        public void TestUnitMatchWithValueOfClause()
        {
            var number = 5;

            number.Match(c => c
                .ValueIs(5).Then(_ => Assert.IsTrue(true))
                .Default(() => Assert.IsTrue(false))
            );
        }

        [TestMethod]
        public void TestUnitMatchWithWhenClause()
        {
            var a = new HasProperty { Number = 5 };

            a.Match(c => c
                .Case(@is => @is.Number == 10, (HasProperty cls) => Assert.IsTrue(false))
                .Case((HasProperty bls) => Assert.IsTrue(true))
                .Default(() => Assert.IsTrue(false))
            );
        }

    }
}
