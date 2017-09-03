using System;

namespace Maple.Core
{
    public interface ISubscriberErrorHandler
    {
        void Handle(IMapleMessage message, Exception exception);
    }
}
