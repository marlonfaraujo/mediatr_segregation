using MediartSegregation.Application.Shared.Abstractions;
using MediartSegregation.Domain.Shared.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace MediartSegregation.Application.Ping.UseCases.CreatePing
{
    public class CreatePingHandler : IRequestApplicationHandler<CreatePingCommand, CreatePingResponse>
    {
        private readonly IDomainNotificationAdapter _notification;

        public CreatePingHandler(IDomainNotificationAdapter notification)
        {
            _notification = notification;
        }

        public Task<CreatePingResponse> Handle(CreatePingCommand request, CancellationToken cancellationToken)
        {
            var message = "Ping command received successfully!";
            var @event = new MediartSegregation.Domain.Entities.Ping(message).CreatePingEvent();
            _notification.Publish(@event, cancellationToken);
            return Task.FromResult(new CreatePingResponse(message));
        }
    }
}
