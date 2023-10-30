
namespace BlazingInject.Core;

public class ServiceCollection : List<ServiceDescriptor>, IServiceCollection
{

    public IServiceCollection AddService(ServiceDescriptor descriptor)
    {
        Add(descriptor);
        return this;
    }

    public IServiceCollection TryAddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        ServiceDescriptor serviceDescriptor = AddServiceDescriptorWithLifetime<TService, TImplementation>(ServiceLifetime.Singleton);

        if (this.All(x => x.ServiceType != typeof(TService)))
        {
            Add(serviceDescriptor);
        }

        return this;
    }

    public IServiceCollection AddSingleton<TService>(Func<ServiceProvider, TService> factory)
        where TService : class
    {
        var serviceDescriptor = new ServiceDescriptor
        {
            ServiceType = typeof(TService),
            ImplementationType = typeof(TService),
            ImplementationFactory = factory,
            Lifetime = ServiceLifetime.Singleton
        };
        Add(serviceDescriptor);
        return this;
    }

    public IServiceCollection AddSingleton(object implementation)
    {
        var serviceType = implementation.GetType();
        
        var serviceDescriptor = new ServiceDescriptor
        {
            ServiceType = serviceType,
            ImplementationType = serviceType,
            Implementation = implementation,
            Lifetime = ServiceLifetime.Singleton
        };
        Add(serviceDescriptor);
        return this;
    }
    
    public IServiceCollection AddSingleton<TService>()
        where TService : class
    {
        ServiceDescriptor serviceDescriptor = AddServiceDescriptorWithLifetime<TService, TService>(ServiceLifetime.Singleton);
        Add(serviceDescriptor);
        return this;
    }
    
    public IServiceCollection AddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        ServiceDescriptor serviceDescriptor = AddServiceDescriptorWithLifetime<TService, TImplementation>(ServiceLifetime.Singleton);
        Add(serviceDescriptor);
        return this;
    }

    public IServiceCollection TryAddTransient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        ServiceDescriptor serviceDescriptor = AddServiceDescriptorWithLifetime<TService, TImplementation>(ServiceLifetime.Transient);

        
        if (this.All(x => x.ServiceType != typeof(TService)))
        {
            Add(serviceDescriptor);
        }

        return this;
    }
    
    public IServiceCollection AddTransient<TService>(Func<ServiceProvider, TService> factory)
        where TService : class
    {
        var serviceDescriptor = new ServiceDescriptor
        {
            ServiceType = typeof(TService),
            ImplementationType = typeof(TService),
            ImplementationFactory = factory,
            Lifetime = ServiceLifetime.Transient
        };
        Add(serviceDescriptor);
        return this;
    }
    
    public IServiceCollection AddTransient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        ServiceDescriptor serviceDescriptor = AddServiceDescriptorWithLifetime<TService, TImplementation>(ServiceLifetime.Transient);
        Add(serviceDescriptor);
        return this;
    }
    
    public IServiceCollection AddTransient<TService>()
        where TService : class
    {
        ServiceDescriptor serviceDescriptor = AddServiceDescriptorWithLifetime<TService, TService>(ServiceLifetime.Transient);
        Add(serviceDescriptor);
        return this;
    }

    private static ServiceDescriptor AddServiceDescriptorWithLifetime<TService, TImplementation>(ServiceLifetime lifetime)
    {
        var serviceDescriptor = new ServiceDescriptor
        {
            ServiceType = typeof(TService),
            ImplementationType = typeof(TImplementation),
            Lifetime = lifetime
        };
        return serviceDescriptor;
    }

    // TODO: weird bc not interface but also where do i get implementation 
    public ServiceProvider BuildServiceProvider()
    {
        CheckTryAdd();
        return new ServiceProvider(this);
    }

    // TODO: Why is this here?
    private void CheckTryAdd()
    {
        foreach (ServiceDescriptor descriptor in this)
        {
            
        }
    }
}