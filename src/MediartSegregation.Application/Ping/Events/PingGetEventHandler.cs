using MediartSegregation.Domain.Events;
using MediartSegregation.Domain.Shared.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace MediartSegregation.Application.Ping.Events
{
    public class PingGetEventHandler : IDomainNotificationHandler<PingGetEvent>
    {
        public Task Handle(PingGetEvent notification, CancellationToken cancellationToken = default)
        {
            var message = $"Ping get with message: {notification.Message}";
            return Task.CompletedTask;
        }
    }
}
