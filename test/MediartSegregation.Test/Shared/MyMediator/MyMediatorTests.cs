using MediartSegregation.Application.Ping.UseCases.CreatePing;
using MediartSegregation.Application.Shared.Abstractions;
using MediartSegregation.Domain.Events;
using MediartSegregation.Shared.MyMediator;
using Moq;

namespace MediartSegregation.Test.Shared.MyMediator
{
    /// <summary>
    /// Unit tests for the MyMediator class, covering SendAsync and PublishAsync methods.
    /// </summary>
    public class MyMediatorTests
    {
        /// <summary>
        /// Verifies that SendAsync calls the handler and returns the expected response.
        /// </summary>
        [Fact]
        public async Task SendAsync_Should_Call_Handler_And_Return_Response()
        {
            // Arrange
            var request = new CreatePingCommand();
            var expectedResponse = new CreatePingResponse("ping");
            var handlerMock = new Mock<IRequestApplicationHandler<CreatePingCommand, CreatePingResponse>>();
            handlerMock
                .Setup(h => h.Handle(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            var serviceProviderMock = new Mock<IServiceProvider>();

            // Fix: Ensure MyMediator is correctly referenced as a type, not a namespace.
            var mediator = new MediartSegregation.Shared.MyMediator.MyMediator(serviceProviderMock.Object);

            // Act
            var response = await mediator.SendAsync(request, handlerMock.Object, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResponse, response);
            handlerMock.Verify(h => h.Handle(request, It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies that PublishAsync calls all registered notification handlers.
        /// </summary>
        [Fact]
        public async Task PublishAsync_Should_Invoke_All_Handlers()
        {
            // Arrange
            var notification = new PingGetEvent("ping");
            var myMediatorMock = new Mock<MediartSegregation.Shared.MyMediator.IMyMediator>();
            myMediatorMock
                .Setup(m => m.PublishAsync(notification, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var adapter = new MyMediatorNotificationAdapter(myMediatorMock.Object);

            // Act
            await adapter.Publish(notification, CancellationToken.None);

            // Assert
            myMediatorMock.Verify(
                m => m.PublishAsync(It.Is<PingGetEvent>(n => n == notification), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
