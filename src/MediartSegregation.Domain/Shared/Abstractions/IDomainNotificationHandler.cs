using System.Threading;
using System.Threading.Tasks;

namespace MediartSegregation.Domain.Shared.Abstractions
{
    public interface IDomainNotificationHandler<TNotification> where TNotification : BaseDomainEvent
    {
        Task Handle(TNotification notification, CancellationToken cancellationToken = default);
    }
}
