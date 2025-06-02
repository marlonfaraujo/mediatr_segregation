using MediartSegregation.Domain.Events;
using MediartSegregation.Domain.Shared.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace MediartSegregation.Application.Ping.Events
{
    public class PingCreatedEventHandler : IDomainNotificationHandler<PingCreatedEvent>
    {

        public Task Handle(PingCreatedEvent notification, CancellationToken cancellationToken = default)
        {
            var message = $"Ping created with message: {notification.Message}";
            return Task.CompletedTask;
        }
    }
}
