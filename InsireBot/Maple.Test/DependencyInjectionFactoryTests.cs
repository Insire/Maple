using DryIoc;
using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Maple.Test
{
    [TestClass]
    public class DependencyInjectionFactoryTests
    {
        private static IContainer _container;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _container = DependencyInjectionFactory.Get();
        }

        [TestMethod]
        public void SanityMapleGetContainerTest()
        {
            _container.VerifyResolutions();
        }

        [TestMethod]
        public void ResolveLoadablesAsList()
        {
            var loadables = _container.Resolve<IList<ILoadableViewModel>>();

            Assert.IsNotNull(loadables);
            Assert.IsTrue(loadables?.Count > 3, $"Only {loadables.Count} instance(s) found");
        }

        [TestMethod]
        public void ResolveManyLoadablesAsList()
        {
            var loadables = _container.ResolveMany<ILoadableViewModel>().ToList();

            Assert.IsNotNull(loadables);
            Assert.IsTrue(loadables?.Count > 3, $"Only {loadables.Count} instance(s) found");
        }
    }
}
