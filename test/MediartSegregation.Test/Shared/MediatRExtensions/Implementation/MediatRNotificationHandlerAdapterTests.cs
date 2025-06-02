using MediartSegregation.Domain.Events;
using MediartSegregation.Domain.Shared.Abstractions;
using MediartSegregation.Shared.MediatRExtensions.Implementation;
using MediatRSegregation.Shared.MediatRExtensions.Implementation;
using Moq;

namespace MediartSegregation.Test.Shared.MediatRExtensions.Implementation
{
    public class MediatRNotificationHandlerAdapterTests
    {
        [Fact]
        public async Task Handle_Should_Delegate_To_DomainNotificationHandler()
        {
            // Arrange
            var handlerMock = new Mock<IDomainNotificationHandler<PingGetEvent>>();
            handlerMock
                .Setup(h => h.Handle(It.IsAny<PingGetEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var adapter = new MediatRNotificationHandlerAdapter<PingGetEvent>(handlerMock.Object);
            var notification = new PingGetEvent("test");
            var domainNotification = new MediatRDomainNotification<PingGetEvent>(notification);

            // Act
            await adapter.Handle(domainNotification, CancellationToken.None);

            // Assert
            handlerMock.Verify(
                h => h.Handle(notification, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
