using MediartSegregation.Application;
using MediartSegregation.Application.Ping.UseCases.CreatePing;
using MediartSegregation.Application.UseCases.Ping.GetPing;
using MediatR;
using MediatRSegregation.Shared.MediatRExtensions;
using MediatRSegregation.Shared.MediatRExtensions.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(Application).Assembly,
        typeof(Program).Assembly
    );
});
builder.Services.AddAdapterRegistrationExtensions(typeof(Application).Assembly);

var app = builder.Build();

var serviceProvider = builder.Services.BuildServiceProvider();
var mediator = serviceProvider.GetRequiredService<IMediator>();

app.MapPost("/ping", async () =>
{
    var createPingResponse = await mediator.Send(new MediatRRequestAdapter<CreatePingCommand, CreatePingResponse>(new CreatePingCommand()));
    var getPingResponse = await mediator.Send(new MediatRRequestAdapter<GetPingQuery, GetPingResponse>(new GetPingQuery()));
    return new { CreateMessage = createPingResponse.Message, GetMessage = getPingResponse.Message };
});

app.Run();