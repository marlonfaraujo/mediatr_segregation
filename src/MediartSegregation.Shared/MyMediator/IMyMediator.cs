using MediartSegregation.Application.Shared.Abstractions;
using MediartSegregation.Domain.Shared.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace MediartSegregation.Shared.MyMediator
{
    public interface IMyMediator
    {
        Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request,
            IRequestApplicationHandler<TRequest, TResponse> requestApplicationHandler,
            CancellationToken cancellationToken = default) where TRequest : IRequestApplication<TResponse>;

        Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : BaseDomainEvent;
    }
}
