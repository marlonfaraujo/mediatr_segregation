using MediartSegregation.Application.Shared.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRSegregation.Shared.MediatRExtensions.Implementation
{
    public class MediatRRequestHandlerAdapter<TRequest, TResult> : IRequestHandler<MediatRRequestAdapter<TRequest, TResult>, TResult> where TRequest : IRequestApplication<TResult>
    {
        private readonly IRequestApplicationHandler<TRequest, TResult> _handler;

        public MediatRRequestHandlerAdapter(IRequestApplicationHandler<TRequest, TResult> handler)
        {
            _handler = handler;
        }

        public Task<TResult> Handle(MediatRRequestAdapter<TRequest,TResult> request, CancellationToken cancellationToken = default)
        {
            return _handler.Handle(request.Request, cancellationToken);
        }
    }
}
