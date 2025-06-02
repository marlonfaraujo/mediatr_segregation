using MediartSegregation.Application.Shared.Abstractions;
using MediartSegregation.Domain.Shared.Abstractions;
using MediartSegregation.Shared.MediatRExtensions.Implementation;
using MediatR;
using MediatRSegregation.Shared.MediatRExtensions.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace MediatRSegregation.Shared.MediatRExtensions
{
    public static class AdapterRegistrationExtensions
    {
        public static IServiceCollection AddAdapterRegistrationExtensions(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddTransient<IDomainNotificationAdapter, MediatRNotificationAdapter>();
            services.RegisterRequestApplication(assemblies, typeof(IDomainNotificationHandler<>));
            services.RegisterRequestApplication(assemblies, typeof(IRequestApplication<>));
            services.RegisterRequestApplication(assemblies, typeof(IRequestApplicationHandler<,>));
            return services;
        }

        private static void RegisterRequestApplication(this IServiceCollection services, Assembly[] assemblies, Type requestInterface)
        {
            foreach (var assembly in assemblies)
            {
                var requests = assembly
                    .GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface)
                    .Select(t => new
                    {
                        Type = t,
                        Interface = t.GetInterfaces()
                            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == requestInterface)
                    })
                    .Where(x => x.Interface != null)
                    .ToList();

                foreach (var request in requests)
                {
                    if (request.Interface.IsGenericType && request.Interface.GetGenericTypeDefinition() == typeof(IRequestApplication<>))
                    {
                        var resultType = request.Interface.GetGenericArguments()[0];
                        var adapterType = typeof(MediatRRequestAdapter<,>).MakeGenericType(request.Type, resultType);
                        var handlerType = typeof(MediatRRequestHandlerAdapter<,>).MakeGenericType(request.Type, resultType);
                        var interfaceType = typeof(IRequestHandler<,>).MakeGenericType(adapterType, resultType);

                        services.AddTransient(interfaceType, handlerType);
                        continue;
                    }

                    if (request.Interface.IsGenericType && request.Interface.GetGenericTypeDefinition() == typeof(IDomainNotificationHandler<>))
                    {                        
                        var resultType = request.Interface.GetGenericArguments()[0];

                        var handlerInterfaceType = typeof(IDomainNotificationHandler<>);
                        var adapterType = typeof(MediatRNotificationHandlerAdapter<>);
                        var notificationWrapperType = typeof(MediatRDomainNotification<>);

                        var domainHandlerType = handlerInterfaceType.MakeGenericType(resultType);
                        var adapterHandlerType = typeof(INotificationHandler<>).MakeGenericType(notificationWrapperType.MakeGenericType(resultType));
                        var adapterImplType = adapterType.MakeGenericType(resultType);

                        services.AddTransient(domainHandlerType, request.Type);
                        services.AddTransient(adapterHandlerType, adapterImplType);

                        continue;                        
                    }

                    services.AddTransient(request.Interface, request.Type);
                }
            }
            
        }
    }
}
