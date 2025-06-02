using MediartSegregation.Domain.Shared.Abstractions;
using MediatR;
using MediatRSegregation.Shared.MediatRExtensions.Implementation;
using System.Threading;
using System.Threading.Tasks;

namespace MediartSegregation.Shared.MediatRExtensions.Implementation
{
    public class MediatRNotificationHandlerAdapter<TNotification>
        : INotificationHandler<MediatRDomainNotification<TNotification>> where TNotification : BaseDomainEvent
    {
        private readonly IDomainNotificationHandler<TNotification> _handler;
        public MediatRNotificationHandlerAdapter(IDomainNotificationHandler<TNotification> handler)
        {
            _handler = handler;
        }

        public Task Handle(MediatRDomainNotification<TNotification> notification, CancellationToken cancellationToken)
        {
            return _handler.Handle(notification.Notification, cancellationToken);
        }
    }
}
