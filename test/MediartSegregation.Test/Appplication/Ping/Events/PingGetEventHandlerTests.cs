using MediartSegregation.Application.Ping.Events;
using MediartSegregation.Domain.Events;

namespace MediartSegregation.Test.Appplication.Ping.Events
{
    public class PingGetEventHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Complete_When_ValidEvent()
        {
            // Arrange
            var handler = new PingGetEventHandler();
            var evt = new PingGetEvent("test message");

            // Act
            var task = handler.Handle(evt, CancellationToken.None);

            // Assert
            await task; // Should complete without exception
        }
    }
}
