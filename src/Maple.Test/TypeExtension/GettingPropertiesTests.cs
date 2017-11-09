using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Maple.Test
{
    [TestClass]
    public sealed class GettingPropertiesTests : Context
    {
        [TestMethod]
        public void When_getting_valid_property_with_given_name()
        {
            typeof(SampleChild).TryGetInstanceProperty("ChildId", out var childIdProp).ShouldBeTrue();
            childIdProp.ShouldNotBeNull();
            childIdProp.PropertyType.ShouldBe(typeof(int));

            typeof(SampleChild).TryGetInstanceProperty("PrivateChildName", out var privateChildNameProp).ShouldBeTrue();
            privateChildNameProp.ShouldNotBeNull();
            privateChildNameProp.PropertyType.ShouldBe(typeof(string));
        }

        [TestMethod]
        public void When_getting_invalid_property_with_given_name()
        {
            typeof(SampleChild).TryGetInstanceProperty("foo", out var someProperty).ShouldBeFalse();
            someProperty.ShouldBeNull();
        }

        [TestMethod]
        public void When_getting_all_public_properties()
        {
            var allProps = typeof(SampleChild).GetInstanceProperties();
            allProps.ShouldNotBeNull();
            allProps.ShouldNotBeEmpty();
            allProps.Length.ShouldBe(8);

            var declaredProps = typeof(SampleChild).GetInstanceProperties(false);
            declaredProps.ShouldNotBeNull();
            declaredProps.ShouldNotBeEmpty();
            declaredProps.Length.ShouldBe(5);
        }
    }
}