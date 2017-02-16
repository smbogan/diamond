using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Diamond.Storage;
using FluentAssertions;

namespace DiamondTest
{
    [TestClass]
    public class ResourceIdentifierTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var r = new ResourceIdentifier(ResourceType.Table, "a", "b33 4", "filename");

            r.Identifier.Should().Be("a/b33 4/filename.table");
        }
    }
}
