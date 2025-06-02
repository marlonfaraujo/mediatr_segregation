using MediartSegregation.Domain.Shared.Abstractions;

namespace MediartSegregation.Domain.Events
{
    public class PingCreatedEvent : BaseDomainEvent
    {
        public string Message { get; }
        public PingCreatedEvent(string message)
        {
            Message = message;
        }
    }
}
