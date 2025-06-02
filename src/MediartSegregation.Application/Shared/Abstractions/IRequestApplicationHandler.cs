using System.Threading;
using System.Threading.Tasks;

namespace MediartSegregation.Application.Shared.Abstractions
{
    public interface IRequestApplicationHandler<TRequest, TResponse> where TRequest : IRequestApplication<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
