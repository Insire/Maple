using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Maple.Test
{
    [TestClass]
    public sealed class GettingAllPropertiesWithAttributes : Context
    {
        [TestInitialize]
        public void TestInitialize()
        {
            When_getting_properties_with_specific_attribute_for_type<SampleChild, MyAttribute>(true);
        }

        [TestMethod]
        public void Then_property_infos_should_not_be_null_or_empty()
        {
            PropertesWithAttribute.ShouldNotBeNull();
            PropertesWithAttribute.ShouldNotBeEmpty();
        }

        [TestMethod]
        public void Then_property_infos_should_have_correct_count()
        {
            PropertesWithAttribute.Count().ShouldBe(3);
        }

        [TestMethod]
        public void Then_correct_attributes_should_have_been_returned()
        {
            PropertesWithAttribute.Single(p => p.Name.Equals("ParentId"))
                .GetCustomAttribute<MyAttribute>()
                .Name.ShouldBe("_parentId");

            PropertesWithAttribute.Single(p => p.Name.Equals("ChildId"))
                .GetCustomAttribute<MyAttribute>()
                .Name.ShouldBe("_childId");

            PropertesWithAttribute.Single(p => p.Name.Equals("ChildAge"))
                .GetCustomAttribute<MyAttribute>()
                .Name.ShouldBe("_childAge");
        }
    }
}