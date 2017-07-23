using DryIoc;
using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;

namespace Maple.Test
{
    [TestClass]
    public class DependencyInjectionFactoryTests
    {

        [TestMethod]
        public async Task SanityMapleGetContainerTest()
        {
            var container = await DependencyInjectionFactory.Get();
            container.VerifyResolutions();
        }

        [TestMethod]
        public async Task ResolveLoadablesAsList()
        {
            var container = await DependencyInjectionFactory.Get();
            var loadables = container.Resolve<IList<ILoadableViewModel>>();

            Assert.IsNotNull(loadables);
            Assert.IsTrue(loadables?.Count > 3, $"Only {loadables.Count} instance(s) found");
        }

        [TestMethod]
        public async Task ResolveManyLoadablesAsList()
        {
            var container = await DependencyInjectionFactory.Get();

            var factory = Substitute.For<IWavePlayerFactory>();
            factory.GetPlayer(Arg.Any<ILoggingService>()).Re
            container.UseInstance(factory);

            var loadables = container.ResolveMany<ILoadableViewModel>().ToList();

            Assert.IsNotNull(loadables);
            Assert.IsTrue(loadables?.Count > 3, $"Only {loadables.Count} instance(s) found");
        }
    }
}
