using MediartSegregation.Application.UseCases.Ping.GetPing;
using MediartSegregation.Domain.Events;
using MediartSegregation.Domain.Shared.Abstractions;
using Moq;

namespace MediartSegregation.Test.Appplication.Ping.UseCases.GetPing
{
    public class GetPingHandlerTests
    {
        [Fact]
        public async Task Handle_Should_PublishEvent_And_ReturnResponse()
        {
            // Arrange
            var notificationMock = new Mock<IDomainNotificationAdapter>();
            notificationMock
                .Setup(n => n.Publish(It.IsAny<PingGetEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new GetPingHandler(notificationMock.Object);
            var query = new GetPingQuery("test");

            // Act
            var response = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equal("Ping query received successfully!", response.Message);
            notificationMock.Verify(n => n.Publish(It.IsAny<PingGetEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
