using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DryIoc;

using Maple.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

namespace Maple.Test
{
    [TestClass]
    public sealed class DependencyInjectionFactoryTests
    {
        [TestMethod]
        public async Task SanityMapleGetContainerTest()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            container.Validate();
        }

        [TestMethod]
        public async Task ResolveMessengerTest()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);

            var messenger = Substitute.For<IMessenger>();
            container.UseInstance(typeof(IMessenger), messenger, IfAlreadyRegistered: IfAlreadyRegistered.Replace, preventDisposal: false, weaklyReferenced: true, serviceKey: "");

            Assert.AreEqual(messenger, container.Resolve<IMessenger>());
        }

        [TestMethod]
        public async Task ResolveLoadablesAsList()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);

            var factory = Substitute.For<IWavePlayerFactory>();
            container.UseInstance(factory);

            var loadables = container.Resolve<IList<ILoadableViewModel>>();

            Assert.IsNotNull(loadables);
            Assert.IsTrue(loadables?.Count > 3, $"Only {loadables.Count} instance(s) found");
        }

        [TestMethod]
        public async Task ResolveManyLoadablesAsList()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);

            var factory = Substitute.For<IWavePlayerFactory>();
            container.UseInstance(factory);

            var loadables = container.ResolveMany<ILoadableViewModel>().ToList();

            Assert.IsNotNull(loadables);
            Assert.IsTrue(loadables?.Count > 3, $"Only {loadables.Count} instance(s) found");
        }
    }
}
