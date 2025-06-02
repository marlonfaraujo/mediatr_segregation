using MediartSegregation.Domain.Events;

namespace MediartSegregation.Domain.Entities
{
    public class Ping
    {
        public string Message { get; private set; } = string.Empty;

        public Ping(string message)
        {
            Message = message;
        }

        public PingCreatedEvent CreatePingEvent()
        {
            return new PingCreatedEvent(Message);
        }

        public PingGetEvent GetPingEvent()
        {
            return new PingGetEvent(Message);
        }
    }
}
