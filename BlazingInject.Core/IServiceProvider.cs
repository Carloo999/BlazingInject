namespace BlazingInject.Core;

public interface IServiceProvider
{
    T? GetService<T>();

    T?[] GetServices<T>();

    object[]? GetServices(Type serviceType);
    
    object? GetService(Type serviceType);
}