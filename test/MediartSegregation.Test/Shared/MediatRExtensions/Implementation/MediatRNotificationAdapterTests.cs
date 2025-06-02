using MediartSegregation.Domain.Events;
using MediatR;
using MediatRSegregation.Shared.MediatRExtensions.Implementation;
using Moq;

namespace MediartSegregation.Test.Shared.MediatRExtensions.Implementation
{
    public class MediatRNotificationAdapterTests
    {
        [Fact]
        public async Task Publish_Should_Call_Mediator_Publish_With_Wrapped_Notification()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Publish(It.IsAny<MediatRDomainNotification<PingGetEvent>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var adapter = new MediatRNotificationAdapter(mediatorMock.Object);
            var notification = new PingGetEvent("test");

            // Act
            await adapter.Publish(notification, CancellationToken.None);

            // Assert
            mediatorMock.Verify(
                m => m.Publish(It.Is<MediatRDomainNotification<PingGetEvent>>(n => n.Notification == notification), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
