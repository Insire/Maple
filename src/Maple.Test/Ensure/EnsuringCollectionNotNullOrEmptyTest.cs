using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Maple.Test
{
    [TestClass]
    public class EnsuringCollectionNotNullOrEmptyTest
    {
        [TestMethod]
        public void When_checking_a_null_string_collection()
        {
            List<string> collection = null;

            Should.Throw<ArgumentNullException>(() =>
            {
                Ensure.NotNullOrEmpty(collection);
            }, "Because collection is null.");
        }

        [TestMethod]
        public void When_checking_an_empty_string_collection()
        {
            var collection = Enumerable.Empty<string>().ToList();

            Action action = () => Ensure.NotNullOrEmpty(collection);

            action.ShouldThrow<ArgumentException>("Because collection is empty.");
        }

        [TestMethod]
        public void When_checking_a_non_empty_collection()
        {
            var collection = new List<string> { "Item One" };

            ICollection<string> returnedValue = new Collection<string>();
            Action action = () => returnedValue = Ensure.NotNullOrEmpty(collection);

            action.ShouldNotThrow("Because collection is not empty.");
            returnedValue.ShouldBeSameAs(collection);
        }
    }
}
