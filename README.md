## About

Framework MediatR and the Mediator pattern

## 1. What is the motivation??
After the news in 2025 that its new versions would be paid, and having recently worked on projects that use it, I had the idea of ​​sharing my experiences with it and how you can try to base your project more on the abstractions of these types of frameworks.

It is currently rare to find projects in which I have participated with the MediatR framework since 2023, in some projects I have used other competitors and in others, my own implementations.

## 2. More about:
In my career with several different teams and projects, I noticed that MediatR was being used in any type of project and to solve any problem. Perhaps the reason is the teams' familiarity with the framework.

In 2023, I returned to participate in some in-person talks and events, after the hiatus we all had during the COVID pandemic. In one of these events, the topic discussed was the mediator pattern and how market frameworks work, including MediatR.

I was able to make the most of this event and, later, I realized that the framework really solved something that could be complex in our projects in a simple way. With a lot of work and many deliveries to make, I never had time to study it and delve deeper into how it works, my mistake.

## 3. Problem:
This may be a problem for some and not for others, but I see the framework being used in various architectures without worrying about dependencies in application and domain layers. And when you have the opportunity to install just the MediatR.Contracts package in more restricted layers, I see full MediatR installations even when it is not needed.

There are also many projects that ignore MediatR's INotification functionality, installing another framework in the same project to be another form of in-memory notification. Additionally, I have seen its use in simpler projects that only perform CRUD operations.

## 4. Digging a little deeper into the subject:
This may be an old view on my part about the framework, but I will focus on summarizing the two implementations:

- INotification
Here I inverted the dependency and instead of implementing INotification directly in a domain layer class for example, I just implement the BaseDomainEvent class in the domain.
```csharp
public class MediatRDomainNotification<TNotification> : INotification where TNotification : BaseDomainEvent
{
    public TNotification Notification { get; }

    public MediatRDomainNotification(TNotification notification)
    {
        Notification = notification;
    }
}
```
- INotificationHandler
For the handler I do the same thing, I have implemented the IDomainNotificationHandler interface in my domain layer instead of the MediatR INotificationHandler interface
```csharp
public class MediatRNotificationHandlerAdapter<TNotification>
    : INotificationHandler<MediatRDomainNotification<TNotification>> where TNotification : BaseDomainEvent
{
    private readonly IDomainNotificationHandler<TNotification> _handler;
    public MediatRNotificationHandlerAdapter(IDomainNotificationHandler<TNotification> handler)
    {
        _handler = handler;
    }

    public Task Handle(MediatRDomainNotification<TNotification> notification, CancellationToken cancellationToken)
    {
        return _handler.Handle(notification.Notification, cancellationToken);
    }
}
```
- IRequest
Same idea as the previous example, I invert the dependency and use the IRequestApplication interface in the application layer instead of the MediatR IRequest interface
```csharp
public class MediatRRequestAdapter<TRequest, TResult> : IRequest<TResult> where TRequest : IRequestApplication<TResult>
{
    public TRequest Request { get; set; }

    public MediatRRequestAdapter(TRequest request)
    {
        Request = request;
    }
}
```
- IRequestHandler
For IRequestHandler I have the implementation of this other interface IRequestApplicationHandler without dependencies with MediatR
```csharp
public class MediatRRequestHandlerAdapter<TRequest, TResult> 
    : IRequestHandler<MediatRRequestAdapter<TRequest, TResult>, TResult> where TRequest : IRequestApplication<TResult>
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
```
For better understanding visit the links:
- Notification: https://github.com/jbogard/MediatR/wiki#notifications
- Request: https://github.com/jbogard/MediatR/wiki#requestresponse

## Delving deeper and deeper into the subject:
The biggest challenge in working with these frameworks is usually in the part of registering classes for dependency injection. This is exactly where they stand out, because the communication used from an API layer to the application layer, for example, has low coupling in these cases due to the use of dependency injection.

And here I will just make a small summary, based on the framework class MediatR/Registration/ServiceRegistrar.cs (https://github.com/jbogard/MediatR/blob/master/src/MediatR/Registration/ServiceRegistrar.cs)
```csharp
public static class ServiceRegistrar
{
  //just a part
  ...
  ...
  private static void ConnectImplementationsToTypesClosing(Type openRequestInterface,
      IServiceCollection services,
      IEnumerable<Assembly> assembliesToScan,
      bool addIfAlreadyExists,
      MediatRServiceConfiguration configuration,
      CancellationToken cancellationToken = default)
  {
      var concretions = new List<Type>();
      var interfaces = new List<Type>();
      var genericConcretions = new List<Type>();
      var genericInterfaces = new List<Type>();
  
      var types = assembliesToScan
          .SelectMany(a => a.DefinedTypes)
          .Where(t => !t.ContainsGenericParameters || configuration.RegisterGenericHandlers)
          .Where(t => t.IsConcrete() && t.FindInterfacesThatClose(openRequestInterface).Any())
          .Where(configuration.TypeEvaluator)
          .ToList();        
  
      foreach (var type in types)
      {
          var interfaceTypes = type.FindInterfacesThatClose(openRequestInterface).ToArray();
  
          if (!type.IsOpenGeneric())
          {
              concretions.Add(type);
  
              foreach (var interfaceType in interfaceTypes)
              {
                  interfaces.Fill(interfaceType);
              }
          }
          else
          {
              genericConcretions.Add(type);
              foreach (var interfaceType in interfaceTypes)
              {
                  genericInterfaces.Fill(interfaceType);
              }
          }
      }
  
      foreach (var @interface in interfaces)
      {
          var exactMatches = concretions.Where(x => x.CanBeCastTo(@interface)).ToList();
          if (addIfAlreadyExists)
          {
              foreach (var type in exactMatches)
              {
                  services.AddTransient(@interface, type);
              }
          }
          else
          {
              if (exactMatches.Count > 1)
              {
                  exactMatches.RemoveAll(m => !IsMatchingWithInterface(m, @interface));
              }
  
              foreach (var type in exactMatches)
              {
                  services.TryAddTransient(@interface, type);
              }
          }
  
          if (!@interface.IsOpenGeneric())
          {
              AddConcretionsThatCouldBeClosed(@interface, concretions, services);
          }
      }
  
      foreach (var @interface in genericInterfaces)
      {
          var exactMatches = genericConcretions.Where(x => x.CanBeCastTo(@interface)).ToList();
          AddAllConcretionsThatClose(@interface, exactMatches, services, assembliesToScan, cancellationToken);
      }
  }
  ...
}
```
It maps the classes that implement IRequestHandler, INotificationHandler, etc, from our example, using the Assembly parameter (there are other options) provided in the MediatR startup configuration as a parameter, and registers the corresponding classes through the service provider, so that .NET can perform dependency injection at runtime. 

Therefore, during startup and depending on the size of the project and the number of classes, it may take some time to go through these registrations. With this, it can resolve, for example, even handler classes that receive other dependencies in their constructor method.

## 5. Conclusion
The main goal was to show that, with dependency inversion, it is possible to isolate MediatR from the application or domain layer. In addition to talking a little about my experience with the framework and some impressions I had in the projects I worked on. Feel free to send any issue or pull request. I'm not very active here, but as soon as I can, I'll respond.
