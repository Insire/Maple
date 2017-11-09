using System;
using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Maple.Test
{
    [TestClass]
    public class EnsuringNotNullTests
    {
        [TestMethod]
        public void When_ensuring_null_string()
        {
            string nullStr = null;
            Should.Throw<ArgumentException>(() => Ensure.NotNull(nullStr, "nullStr"));
        }

        [TestMethod]
        public void When_ensuring_null_object()
        {
            object nullObj = null;
            Should.Throw<ArgumentException>(() => Ensure.NotNull(nullObj, "nullObj"));
        }

        [TestMethod]
        public void When_ensuring_non_null_object()
        {
            object nonNullObj = 1;
            Should.NotThrow(() => Ensure.NotNull(nonNullObj, "nonNullObj"),
                "Because nonNullObject is not null");
        }

        [TestMethod]
        public void When_ensuring_non_null_object_with_null_argument_name()
        {
            object anyObject = 120;
            Should.NotThrow(() => Ensure.NotNull(anyObject, null),
                "Because object is not null");
        }

        [TestMethod]
        public void When_ensuring_null_object_with_null_argument_name()
        {
            object nullObject = null;
            Should.Throw<ArgumentException>(() => Ensure.NotNull(nullObject, null));
        }
    }
}
