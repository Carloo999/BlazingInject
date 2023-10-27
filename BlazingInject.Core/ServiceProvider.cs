using System.Collections;
using System.Reflection;

namespace BlazingInject.Core;

public class ServiceProvider
{
    private readonly Dictionary<Type, List<Func<object>>> _transientTypes = new();
    private readonly Dictionary<Type, List<Lazy<object>>> _singletonTypes = new();
    internal ServiceProvider(ServiceCollection serviceCollection)
    {
        GenerateServices(serviceCollection);
    }

    public T? GetService<T>()
    {
        return (T?)GetService(typeof(T));
    }

    public T?[] GetServices<T>()
    {  
        var services = GetServices(typeof(T));
        var servicesCasted = new List<T?>();

        foreach (var service in services)
        {
            servicesCasted.Add((T?)service);
        }

        return servicesCasted.ToArray();
    }

    public object[]? GetServices(Type serviceType)
    {
        var singletonService = _singletonTypes.GetValueOrDefault(serviceType)?.Select(x => x.Value);
        var services = new List<object>();
        
        if (singletonService is not null)
        {
            services = services.Concat(singletonService).ToList();
        }

        var transientService = _transientTypes.GetValueOrDefault(serviceType);

        if (transientService is null) return services.ToArray();
        
        foreach (Func<object> transient in transientService)
        {
            services.Add(transient.Invoke());
        }

        return services.ToArray();
    }

    public object? GetService(Type serviceType)
    {
        Lazy<object>? singletonService = _singletonTypes.GetValueOrDefault(serviceType)?.Last();

        if (singletonService is not null)
        {
            return singletonService.Value;
        }

        var transientService = _transientTypes.GetValueOrDefault(serviceType)?.Last(); 
        return transientService?.Invoke();
    }

    private void GenerateServices(ServiceCollection serviceCollection)
    {
        foreach (ServiceDescriptor serviceDescriptor in serviceCollection)
        {
            switch (serviceDescriptor.Lifetime)
            {
                case ServiceLifetime.Singleton:

                    if (_singletonTypes.GetValueOrDefault(serviceDescriptor.ServiceType) is null)
                    {
                        _singletonTypes[serviceDescriptor.ServiceType] = new List<Lazy<object>>();
                    }
                    
                    if (serviceDescriptor.Implementation is not null)
                    {
                        _singletonTypes[serviceDescriptor.ServiceType].
                            Add(new Lazy<object>(serviceDescriptor.Implementation));   
                        continue;
                    }

                    if (serviceDescriptor.ImplementationFactory is not null)
                    {
                        _singletonTypes[serviceDescriptor.ServiceType].
                            Add(new Lazy<object>(() =>
                            serviceDescriptor.ImplementationFactory(this)));
                        continue;
                    }
                    
                    _singletonTypes[serviceDescriptor.ServiceType].
                        Add(new Lazy<object>(() => 
                            Activator.CreateInstance(serviceDescriptor.ImplementationType,
                                GetConstructorParameters(serviceDescriptor))!));
                    continue;
                case ServiceLifetime.Transient:

                    if (_transientTypes.GetValueOrDefault(serviceDescriptor.ServiceType) is null)
                    {
                        _transientTypes[serviceDescriptor.ServiceType] = new List<Func<object>>();
                    }
                    
                    if (serviceDescriptor.ImplementationFactory is not null)
                    {
                        _transientTypes[serviceDescriptor.ServiceType].
                            Add(() => serviceDescriptor.ImplementationFactory(this));
                        continue;
                    }

                    _transientTypes[serviceDescriptor.ServiceType].
                        Add(() => Activator.CreateInstance(serviceDescriptor.ImplementationType,
                            GetConstructorParameters(serviceDescriptor))!);
                    continue;
            }
        }
    }

    private object?[] GetConstructorParameters(ServiceDescriptor serviceDescriptor)
    {
        ConstructorInfo constructorInfo = serviceDescriptor.ImplementationType.GetConstructors().First();
        var parameters = constructorInfo.GetParameters()
            .Select(x => GetService(x.ParameterType)).ToArray();

        return parameters;
    }
}