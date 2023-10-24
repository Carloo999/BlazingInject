
namespace BlazingInject.Core;

public class ServiceCollection : List<ServiceDescriptor>
{

    public ServiceCollection AddSingleton<TService, TImplementation>()
    {
        ServiceDescriptor serviceDescriptor = AddServiceDescriptorWithLifetime<TService, TImplementation>(ServiceLifetime.Singleton);
        Add(serviceDescriptor);
        return this;
    }
    
    public ServiceCollection AddTransient<TService, TImplementation>()
    {
        ServiceDescriptor serviceDescriptor = AddServiceDescriptorWithLifetime<TService, TImplementation>(ServiceLifetime.Transient);
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

    public ServiceProvider BuildServiceProvider()
    {
        return new ServiceProvider(this);
    }
}