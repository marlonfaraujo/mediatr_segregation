using MediartSegregation.Application.Shared.Abstractions;
using MediatR;

namespace MediatRSegregation.Shared.MediatRExtensions.Implementation
{
    public class MediatRRequestAdapter<TRequest, TResult> : IRequest<TResult> where TRequest : IRequestApplication<TResult>
    {
        public TRequest Request { get; set; }

        public MediatRRequestAdapter(TRequest request)
        {
            Request = request;
        }
    }
}
