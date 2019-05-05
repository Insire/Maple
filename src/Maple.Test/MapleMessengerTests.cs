using System;

using Maple.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maple.Test
{
    [TestClass, Ignore]
    public static class MapleMessengerTests
    {
        //[TestMethod]
        //public void Dispose_WithValidHubReference_UnregistersWithHub()
        //{
        //    var messengerMock = NSubstitute.Substitute.For<IMapleMessengerHub>();
        //    messengerMock.Setup((messenger) => messenger.Unsubscribe<TestMessage>(Moq.It.IsAny<TinyMessageSubscriptionToken>())).Verifiable();
        //    var token = new MapleMessageSubscriptionToken(messengerMock, typeof(TestMessage));

        //    token.Dispose();

        //    messengerMock.VerifyAll();
        //}

        //[TestMethod]
        //public void Dispose_WithInvalidHubReference_DoesNotThrow()
        //{
        //    var token = UtilityMethods.GetTokenWithOutOfScopeMessenger();
        //    GC.Collect();
        //    GC.WaitForFullGCComplete(2000);

        //    token.Dispose();
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void Ctor_NullHub_ThrowsArgumentNullException()
        //{
        //    var messenger = UtilityMethods.GetMessenger();

        //    var token = new MapleMessageSubscriptionToken(null, typeof(IMapleMessage));
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentOutOfRangeException))]
        //public void Ctor_InvalidMessageType_ThrowsArgumentOutOfRangeException()
        //{
        //    var messenger = UtilityMethods.GetMessenger();

        //    var token = new MapleMessageSubscriptionToken(messenger, typeof(object));
        //}

        //[TestMethod]
        //public void Ctor_ValidHubAndMessageType_DoesNotThrow()
        //{
        //    var messenger = UtilityMethods.GetMessenger();

        //    var token = new MapleMessageSubscriptionToken(messenger, typeof(TestMessage));
        //}

        public class TestMessage : MapleMessageBase
        {
            public TestMessage(object sender) : base(sender)
            {
            }
        }

        public class DerivedMessage<TThings> : TestMessage
        {
            public TThings Things { get; set; }

            public DerivedMessage(object sender)
                : base(sender)
            {
            }
        }

        public interface ITestMessageInterface : IMapleMessage
        {
        }

        public class InterfaceDerivedMessage<TThings> : ITestMessageInterface
        {
            public object Sender { get; private set; }

            public TThings Things { get; set; }

            public InterfaceDerivedMessage(object sender)
            {
                Sender = sender;
            }
        }

        public class TestProxy : IMapleMessageProxy
        {
            public IMapleMessage Message { get; private set; }

            public void Deliver(IMapleMessage message, IMapleMessageSubscription subscription)
            {
                Message = message;
                subscription.Deliver(message);
            }
        }

        public class TestSubscriptionErrorHandler : ISubscriberErrorHandler
        {
            public void Handle(IMapleMessage message, Exception exception)
            {
                throw exception;
            }
        }

        public static class UtilityMethods
        {
            //public static IMapleMessengerHub GetMessenger()
            //{
            //    return new MapleMessengerHub();
            //}

            //public static IMapleMessengerHub GetMessengerWithSubscriptionErrorHandler()
            //{
            //    return new MapleMessengerHub(new TestSubscriptionErrorHandler());
            //}

            public static void FakeDeliveryAction<T>(T message)
                where T : IMapleMessage
            {
            }

            public static bool FakeMessageFilter<T>(T message)
                where T : IMapleMessage
            {
                return true;
            }

            //public static MapleMessageSubscriptionToken GetTokenWithOutOfScopeMessenger()
            //{
            //    var messenger = UtilityMethods.GetMessenger();

            //    var token = new MapleMessageSubscriptionToken(messenger, typeof(TestMessage));

            //    return token;
            //}
        }
    }
}
