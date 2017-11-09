using System;
using Maple.Core;
using Maple.Localization.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Maple.Test
{
    [TestClass]
    public class EnsuringThatTests
    {
        [TestMethod]
        public void When_ensuring_true_condition()
        {
            Should.NotThrow(() => Ensure.That(true), Resources.ExceptionMessageTrueCondition);
        }

        [TestMethod]
        public void When_ensuring_true_condition_with_custom_exception()
        {
            Should.NotThrow(() => Ensure.That<ApplicationException>(true), Resources.ExceptionMessageTrueCondition);
        }

        [TestMethod]
        public void When_ensuring_true_condition_with_custom_message()
        {
            Should.NotThrow(() => Ensure.That(true), Resources.ExceptionMessageTrueCondition);
        }

        [TestMethod]
        public void When_ensuring_false_condition_with_default_exception()
        {
            Should.Throw<ArgumentException>(() => Ensure.That(false), Resources.ExceptionMessageFalseCondition).Message.ShouldBe(Resources.ExceptionMessageFalseCondition);
        }

        [TestMethod]
        public void When_ensuring_false_condition_with_custom_exception()
        {
            Should.Throw<ApplicationException>(() => Ensure.That<ApplicationException>(false)).Message.ShouldBe(Resources.ExceptionMessageFalseCondition);
        }

        [TestMethod]
        public void When_ensuring_false_condition_with_custom_message()
        {
            Should.Throw<ApplicationException>(() => Ensure.That<ApplicationException>(false, "Cause I say so!"), Resources.ExceptionMessageFalseCondition).Message.ShouldBe("Cause I say so!");
        }
    }
}
