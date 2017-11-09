using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Maple.Test
{
    [TestClass]
    public sealed class GettingParentPropertiesWithAttributes : Context
    {
        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void Then_property_infos_should_not_be_null_or_empty(bool inherit)
        {
            When_getting_properties_with_specific_attribute_for_type<SampleParent, MyAttribute>(inherit);

            PropertesWithAttribute.ShouldNotBeNull();
            PropertesWithAttribute.ShouldNotBeEmpty();
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void Then_property_infos_should_have_correct_count(bool inherit)
        {
            When_getting_properties_with_specific_attribute_for_type<SampleParent, MyAttribute>(inherit);

            PropertesWithAttribute.Count().ShouldBe(1);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void Then_correct_attributes_should_have_been_returned(bool inherit)
        {
            When_getting_properties_with_specific_attribute_for_type<SampleParent, MyAttribute>(inherit);

            PropertesWithAttribute.Single(p => p.Name.Equals("ParentId"))
                .GetCustomAttribute<MyAttribute>()
                .Name.ShouldBe("_parentId");
        }
    }
}