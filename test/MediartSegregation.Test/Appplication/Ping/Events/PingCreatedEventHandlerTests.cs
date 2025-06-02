using MediartSegregation.Application.Ping.Events;
using MediartSegregation.Domain.Events;

namespace MediartSegregation.Test.Appplication.Ping.Events
{
    public class PingCreatedEventHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Complete_When_ValidEvent()
        {
            // Arrange
            var handler = new PingCreatedEventHandler();
            var evt = new PingCreatedEvent("test message");

            // Act
            var task = handler.Handle(evt, CancellationToken.None);

            // Assert
            await task; // Should complete without exception
        }
    }
}
