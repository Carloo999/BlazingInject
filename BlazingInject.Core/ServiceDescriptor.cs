namespace BlazingInject.Core;

public class ServiceDescriptor
{
    public ServiceLifetime Lifetime { get; init; }
    public Type ServiceType { get; init; } = default!;
    public Type ImplementationType { get; set; }
    public Func<ServiceProvider, object>? ImplementationFactory { get; set; }
    public object? Implementation { get; set; }

}