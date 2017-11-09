using System;
using Maple.Core;
using Maple.Localization.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Maple.Test
{
    [TestClass]
    public class EnsuringNotEqualTests
    {
        [TestMethod]
        public void When_comparing_equal_strings()
        {
            const string FirstString = "One";
            const string SecondString = "One";

            Action action = () => Ensure.NotEqual(FirstString, SecondString);

            action.ShouldThrow<ArgumentException>("Because the two strings are equal").Message.ShouldBe(Resources.ExceptionMessageNotEqualValues);
        }

        [TestMethod]
        public void When_comparing_equal_integers()
        {
            const int FirstInteger = 1;
            const int SecondInteger = 1;

            Action action = () => Ensure.NotEqual(FirstInteger, SecondInteger);

            action.ShouldThrow<ArgumentException>("Because the two integers are equal").Message.ShouldBe(Resources.ExceptionMessageNotEqualValues);
        }

        [TestMethod]
        public void When_comparing_different_types_with_equal_values()
        {
            const string FirstString = "One";
            object secondString = "One";

            Action action = () => Ensure.NotEqual(FirstString, secondString);

            action.ShouldThrow<ArgumentException>("Because the two objects are equal").Message.ShouldBe(Resources.ExceptionMessageNotEqualValues);
        }

        [TestMethod]
        public void When_comparing_different_strings()
        {
            const string FirstString = "One";
            const string SecondString = "Two";

            Action action = () => Ensure.NotEqual(FirstString, SecondString);

            action.ShouldNotThrow("Because the two strings are different");
        }

        [TestMethod]
        public void When_comparing_different_integers()
        {
            const int FirstInteger = 1;
            const int SecondInteger = 2;

            Action action = () => Ensure.NotEqual(FirstInteger, SecondInteger);

            action.ShouldNotThrow("Because the two integers are different");
        }

        [TestMethod]
        public void When_comparing_string_against_null_object()
        {
            const string FirstString = "Sample";
            string secondString = null;

            Action action = () => Ensure.NotEqual(FirstString, secondString);

            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_comparing_null_object_against_string()
        {
            string firstString = null;
            const string SecondString = "Sample";

            Action action = () => Ensure.NotEqual(firstString, SecondString);

            action.ShouldThrow<NullReferenceException>("Because the first object is null");
        }

        [TestMethod]
        public void When_comparing_two_null_objects()
        {
            string firstString = null;
            string secondSTring = null;

            Action action = () => Ensure.NotEqual(firstString, secondSTring);

            action.ShouldThrow<NullReferenceException>();
        }
    }
}
