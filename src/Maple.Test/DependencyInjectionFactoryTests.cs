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
            container = container.ConfigureForTesting();

            container.VerifyResolutions();
        }

        [TestMethod]
        public async Task ResolveMessengerTest()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            container = container.ConfigureForTesting();

            var messenger = Substitute.For<IMessenger>();
            container.UseInstance(typeof(IMessenger), messenger, IfAlreadyRegistered: IfAlreadyRegistered.Replace);

            Assert.AreEqual(messenger, container.Resolve<IMessenger>());
        }

        [TestMethod]
        public async Task ResolveLoadablesAsList()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            container = container.ConfigureForTesting();

            var loadables = container.Resolve<IList<ILoadableViewModel>>();

            Assert.IsNotNull(loadables);
            Assert.IsTrue(loadables?.Count > 3, $"Only {loadables.Count} instance(s) found");
        }

        [TestMethod]
        public async Task ResolveManyLoadablesAsList()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            container = container.ConfigureForTesting();

            var loadables = container.ResolveMany<ILoadableViewModel>().ToList();

            Assert.IsNotNull(loadables);
            Assert.IsTrue(loadables?.Count > 3, $"Only {loadables.Count} instance(s) found");
        }
    }
}
