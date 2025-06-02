using MediartSegregation.Application;
using MediartSegregation.Application.Ping.UseCases.CreatePing;
using MediatR;
using MediatRSegregation.Shared.MediatRExtensions;
using MediatRSegregation.Shared.MediatRExtensions.Implementation;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(Application).Assembly,
        typeof(Program).Assembly
    );
});

services.AddAdapterRegistrationExtensions(typeof(Application).Assembly);

var serviceProvider = services.BuildServiceProvider();
var mediator = serviceProvider.GetRequiredService<IMediator>();

var response = await mediator.Send(new MediatRRequestAdapter<CreatePingCommand, CreatePingResponse>(new CreatePingCommand()));
Console.WriteLine($"MediatR request sent {response.Message}.");



