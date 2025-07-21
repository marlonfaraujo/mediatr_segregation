using MediartSegregation.Application.Shared.Abstractions;
using MediartSegregation.Domain.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediartSegregation.Shared.MyMediator
{
    public class MyMediator : IMyMediator
    {
        private readonly IServiceProvider _provider;

        public MyMediator(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<TResponse> SendAsync<TRequest,TResponse>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequestApplication<TResponse>
        {
            var handlerType = typeof(IRequestApplicationHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            if (handlerType == null)
            {
                throw new InvalidOperationException("No handler found for type.");
            }
            var handler = _provider.GetService(handlerType);
            if( handler == null)
            {
                throw new InvalidOperationException($"Handler for type {handlerType.Name} not found.");
            }
            var method = handlerType.GetMethod("Handle");
            var result = await (Task<TResponse>)method.Invoke(handler, new object[] { request, cancellationToken });
            return result;
        }

        public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : BaseDomainEvent
        {
            var handlerType = typeof(IDomainNotificationHandler<>).MakeGenericType(notification.GetType());
            var handlers = _provider.GetServices(handlerType);
            foreach (var handler in handlers)
            {
                if (!handlerType.IsAssignableFrom(handler.GetType()))
                {
                    throw new InvalidOperationException($"No handler found for notification type.");
                }
                var method = handlerType.GetMethod("Handle");
                await (Task)method.Invoke(handler, new object[] { notification, cancellationToken });
            }
        }
    }
}
