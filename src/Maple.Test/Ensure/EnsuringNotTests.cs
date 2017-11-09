using System;
using Maple.Core;
using Maple.Localization.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Maple.Test
{
    [TestClass]
    public class EnsuringNotTests
    {
        [TestMethod]
        public void When_ensuring_true_condition()
        {
            Should.Throw<ArgumentException>(() => Ensure.Not(true)).Message.ShouldBe(Resources.ExceptionMessageFalseCondition);
        }

        [TestMethod]
        public void When_ensuring_true_condition_with_custom_exception()
        {
            Should.Throw<ApplicationException>(() => Ensure.Not<ApplicationException>(true)).Message.ShouldBe(Resources.ExceptionMessageTrueCondition);
        }

        [TestMethod]
        public void When_ensuring_true_condition_with_custom_message()
        {
            Should.Throw<ArgumentException>(() => Ensure.Not(true, "Cause I say so!")).Message.ShouldBe("Cause I say so!");
        }

        [TestMethod]
        public void When_ensuring_false_condition_with_default_exception()
        {
            Should.NotThrow(() => Ensure.Not(false), Resources.ExceptionMessageFalseCondition);
        }

        [TestMethod]
        public void When_ensuring_false_condition_with_custom_exception()
        {
            Should.Throw<ApplicationException>(() => Ensure.Not<ApplicationException>(true)).Message.ShouldBe(Resources.ExceptionMessageTrueCondition);
        }

        [TestMethod]
        public void When_ensuring_false_condition_with_custom_message()
        {
            Should.Throw<ApplicationException>(() => Ensure.Not<ApplicationException>(true, "Cause I say so!")).Message.ShouldBe("Cause I say so!");
        }
    }
}
