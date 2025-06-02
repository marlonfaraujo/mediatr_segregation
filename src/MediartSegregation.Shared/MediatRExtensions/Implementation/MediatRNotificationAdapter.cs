using MediartSegregation.Domain.Shared.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRSegregation.Shared.MediatRExtensions.Implementation
{
    public class MediatRNotificationAdapter : IDomainNotificationAdapter
    {
        private readonly IMediator _mediator;

        public MediatRNotificationAdapter(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : BaseDomainEvent
        {
           await _mediator.Publish(new MediatRDomainNotification<TNotification>(notification), cancellationToken);
        }
    }
}
