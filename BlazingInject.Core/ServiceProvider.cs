using System.Reflection;

namespace BlazingInject.Core;

public class ServiceProvider
{
    private readonly Dictionary<Type, Func<object>> _transientTypes = new();
    private readonly Dictionary<Type, Lazy<object>> _singletonTypes = new();
    internal ServiceProvider(ServiceCollection serviceCollection)
    {
        GenerateServices(serviceCollection);
    }

    public T? GetService<T>()
    {
        return (T?)GetService(typeof(T));
    }

    public object? GetService(Type serviceType)
    {
        var singletonService = _singletonTypes.GetValueOrDefault(serviceType);

        if (singletonService is not null)
        {
            return singletonService.Value;
        }

        var transientService = _transientTypes.GetValueOrDefault(serviceType);
        return transientService?.Invoke();
    }

    private void GenerateServices(ServiceCollection serviceCollection)
    {
        foreach (ServiceDescriptor serviceDescriptor in serviceCollection)
        {
            switch (serviceDescriptor.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    if (serviceDescriptor.Implementation is not null)
                    {
                        _singletonTypes[serviceDescriptor.ServiceType] = 
                            new Lazy<object>(serviceDescriptor.Implementation);
                        continue;
                    }

                    if (serviceDescriptor.ImplementationFactory is not null)
                    {
                        _singletonTypes[serviceDescriptor.ServiceType] =
                            new Lazy<object>(() =>
                                serviceDescriptor.ImplementationFactory(this));
                        continue;
                    }
                    
                    _singletonTypes[serviceDescriptor.ServiceType] =
                        new Lazy<object>(() => 
                            Activator.CreateInstance(serviceDescriptor.ImplementationType,
                            GetConstructorParameters(serviceDescriptor))!);
                    continue;
                case ServiceLifetime.Transient:

                    if (serviceDescriptor.ImplementationFactory is not null)
                    {
                        _transientTypes[serviceDescriptor.ServiceType] =
                            () => serviceDescriptor.ImplementationFactory(this);
                        continue;
                    }

                    _transientTypes[serviceDescriptor.ServiceType] =
                        () => Activator.CreateInstance(serviceDescriptor.ImplementationType,
                                GetConstructorParameters(serviceDescriptor))!;
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