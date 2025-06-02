using MediartSegregation.Application.Ping.UseCases.CreatePing;
using MediartSegregation.Application.Shared.Abstractions;
using MediatRSegregation.Shared.MediatRExtensions.Implementation;
using Moq;

namespace MediartSegregation.Test.Shared.MediatRExtensions.Implementation
{
    public class MediatRRequestHandlerAdapterTests
    {
        [Fact]
        public async Task Handle_Should_Call_Handler_And_Return_Result()
        {
            // Arrange
            var request = new CreatePingCommand();
            var expectedResult = "result";

            var handlerMock = new Mock<IRequestApplicationHandler<CreatePingCommand, CreatePingResponse>>();

            handlerMock
                .Setup(h => h.Handle(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreatePingResponse(expectedResult)); // Pass 'message' argument to constructor

            var adapter = new MediatRRequestHandlerAdapter<CreatePingCommand, CreatePingResponse>(handlerMock.Object);
            var requestAdapter = new MediatRRequestAdapter<CreatePingCommand, CreatePingResponse>(request);

            // Act
            var response = await adapter.Handle(requestAdapter, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult, response.Message);
            handlerMock.Verify(h => h.Handle(request, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
