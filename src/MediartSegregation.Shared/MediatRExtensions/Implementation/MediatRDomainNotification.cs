using MediartSegregation.Domain.Shared.Abstractions;
using MediatR;

namespace MediatRSegregation.Shared.MediatRExtensions.Implementation
{
    public class MediatRDomainNotification<TNotification> : INotification where TNotification : BaseDomainEvent
    {
        public TNotification Notification { get; }

        public MediatRDomainNotification(TNotification notification)
        {
            Notification = notification;
        }

    }
}
