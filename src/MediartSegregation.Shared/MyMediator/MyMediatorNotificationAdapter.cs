using MediartSegregation.Domain.Shared.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace MediartSegregation.Shared.MyMediator
{
    public class MyMediatorNotificationAdapter : IDomainNotificationAdapter 
    {
        private readonly MyMediator _mediator;

        public MyMediatorNotificationAdapter(MyMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken) where TNotification : BaseDomainEvent
        {
            await _mediator.PublishAsync<TNotification>(notification, cancellationToken);
        }
    }
}
