using MediartSegregation.Application.Ping.UseCases.CreatePing;
using MediartSegregation.Domain.Events;
using MediartSegregation.Domain.Shared.Abstractions;
using Moq;

namespace MediartSegregation.Test.Appplication.Ping.UseCases.CreatePing
{
    public class CreatePingHandlerTests
    {
        [Fact]
        public async Task Handle_Should_PublishEvent_And_ReturnResponse()
        {
            // Arrange
            var notificationMock = new Mock<IDomainNotificationAdapter>();
            notificationMock
                .Setup(n => n.Publish(It.IsAny<PingCreatedEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new CreatePingHandler(notificationMock.Object);
            var command = new CreatePingCommand("test");

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equal("Ping command received successfully!", response.Message);
            notificationMock.Verify(n => n.Publish(It.IsAny<PingCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
