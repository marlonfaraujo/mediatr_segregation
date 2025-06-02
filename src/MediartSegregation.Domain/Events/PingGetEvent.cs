using MediartSegregation.Domain.Shared.Abstractions;

namespace MediartSegregation.Domain.Events
{
    public class PingGetEvent : BaseDomainEvent
    {
        public string Message { get; }
        public PingGetEvent(string message)
        {
            Message = message;
        }
    }
}
