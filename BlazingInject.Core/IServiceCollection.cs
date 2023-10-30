namespace BlazingInject.Core;

public interface IServiceCollection : IList<ServiceDescriptor>
{
    IServiceCollection AddService(ServiceDescriptor descriptor);
    
    IServiceCollection TryAddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;
    
    IServiceCollection AddSingleton<TService>(Func<ServiceProvider, TService> factory)
        where TService : class;
    
    IServiceCollection AddSingleton(object implementation);
    
    IServiceCollection AddSingleton<TService>()
        where TService : class;
    
    IServiceCollection AddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;
    
    IServiceCollection TryAddTransient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;
    
    IServiceCollection AddTransient<TService>(Func<ServiceProvider, TService> factory)
        where TService : class;
    
    IServiceCollection AddTransient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;
    
    IServiceCollection AddTransient<TService>()
        where TService : class;

    ServiceProvider BuildServiceProvider();
}