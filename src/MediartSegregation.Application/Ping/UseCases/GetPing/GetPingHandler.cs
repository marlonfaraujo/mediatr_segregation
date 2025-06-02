using MediartSegregation.Application.Shared.Abstractions;
using MediartSegregation.Domain.Shared.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace MediartSegregation.Application.UseCases.Ping.GetPing
{
    public class GetPingHandler : IRequestApplicationHandler<GetPingQuery, GetPingResponse>
    {
        private readonly IDomainNotificationAdapter _notification;

        public GetPingHandler(IDomainNotificationAdapter notification)
        {
            _notification = notification;
        }
        public Task<GetPingResponse> Handle(GetPingQuery request, CancellationToken cancellationToken = default)
        {
            var message = "Ping query received successfully!";
            var @event = new MediartSegregation.Domain.Entities.Ping(message).GetPingEvent();
            _notification.Publish(@event, cancellationToken);
            return Task.FromResult(new GetPingResponse(message));
        }
    }
}
