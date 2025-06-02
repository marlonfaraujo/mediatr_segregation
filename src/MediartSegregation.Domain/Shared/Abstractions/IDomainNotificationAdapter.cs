using System.Threading;
using System.Threading.Tasks;

namespace MediartSegregation.Domain.Shared.Abstractions
{
    public interface IDomainNotificationAdapter
    {
        Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken) where TNotification : BaseDomainEvent;
    }
}
