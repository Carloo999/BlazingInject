
using BlazingInject.Tests.Unit.TestUtils;

namespace BlazingInject.Tests.Unit;

public class ServiceCollectionTests
{
    private readonly IServiceCollection _sut = new ServiceCollection();

    [Fact]
    public void AddService_Should_Add_ServiceDescriptor_To_ServiceCollection()
    {
        // Arrange
        var serviceDescriptor = new ServiceDescriptor { ServiceType = typeof(ITestService), ImplementationType = typeof(TestImplementation) };

        // Act
        _sut.AddService(serviceDescriptor);

        // Assert
        _sut.Count.Should().Be(1);
        _sut.Should().Contain(serviceDescriptor);
    }
    
    [Fact]
    public void AddSingleton_Should_Add_ServiceDescriptor_To_ServiceCollection()
    {
        // Act
        _sut.AddSingleton<ITestService, TestImplementation>();

        // Assert
        _sut.Count.Should().Be(1);
        ServiceDescriptor descriptor = _sut.First();
        descriptor.ServiceType.Should().Be(typeof(ITestService));
        descriptor.ImplementationType.Should().Be(typeof(TestImplementation));
        descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }
    
    [Fact]
    public void AddSingleton_With_Factory_Should_Add_ServiceDescriptor_To_ServiceCollection()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        
        // Act
        serviceCollection.AddSingleton<ITestService>(factory => new TestImplementation());

        // Assert
        serviceCollection.Count.Should().Be(1);
        ServiceDescriptor descriptor = serviceCollection.First();
        descriptor.ServiceType.Should().Be(typeof(ITestService));
        descriptor.ImplementationType.Should().Be(typeof(ITestService));
        descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
        descriptor.ImplementationFactory.Should().NotBeNull();
    }
    
    [Fact]
    public void AddTransient_Should_Add_ServiceDescriptor_To_ServiceCollection()
    {
        // Act
        _sut.AddTransient<ITestService, TestImplementation>();

        // Assert
        _sut.Count.Should().Be(1);
        
        ServiceDescriptor descriptor = _sut.First();
        
        descriptor.ServiceType.Should().Be(typeof(ITestService));
        descriptor.ImplementationType.Should().Be(typeof(TestImplementation));
        descriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
    }
    
    [Fact]
    public void TryAddSingleton_When_Service_Not_Present_Should_Add_ServiceDescriptor_To_ServiceCollection()
    {
        // Act
        _sut.TryAddSingleton<ITestService, TestImplementation>();

        // Assert
        _sut.Count.Should().Be(1);
        ServiceDescriptor descriptor = _sut.First();
        descriptor.ServiceType.Should().Be(typeof(ITestService));
        descriptor.ImplementationType.Should().Be(typeof(TestImplementation));
        descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }
    
    [Fact]
    public void TryAddSingleton_When_Service_Already_Present_Should_Not_Add_ServiceDescriptor_To_ServiceCollection()
    {
        // Arrange
        _sut.TryAddSingleton<ITestService, TestImplementation>();
        
        // Act
        _sut.TryAddSingleton<ITestService, AnotherTestImplementation>();

        // Assert
        _sut.Count.Should().Be(1);
        ServiceDescriptor descriptor = _sut.First();
        descriptor.ServiceType.Should().Be(typeof(ITestService));
        descriptor.ImplementationType.Should().Be(typeof(TestImplementation));
        descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }
    
    [Fact]
    public void TryAddTransient_When_Service_Not_Present_Should_Add_ServiceDescriptor_To_ServiceCollection()
    {
        // Act
        _sut.TryAddTransient<ITestService, TestImplementation>();

        // Assert
        _sut.Count.Should().Be(1);
        ServiceDescriptor descriptor = _sut.First();
        descriptor.ServiceType.Should().Be(typeof(ITestService));
        descriptor.ImplementationType.Should().Be(typeof(TestImplementation));
        descriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
    }
    
    [Fact]
    public void TryAddTransient_When_Service_Already_Present_Should_Not_Add_ServiceDescriptor_To_ServiceCollection()
    {
        // Arrange
        _sut.TryAddTransient<ITestService, TestImplementation>();

        // Act
        _sut.TryAddTransient<ITestService, AnotherTestImplementation>();

        // Assert
        _sut.Count.Should().Be(1);
        ServiceDescriptor descriptor = _sut.First();
        descriptor.ServiceType.Should().Be(typeof(ITestService));
        descriptor.ImplementationType.Should().Be(typeof(TestImplementation));
        descriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
    }
    
    [Fact]
    public void AddTransient_With_Factory_Should_Add_ServiceDescriptor_To_ServiceCollection()
    {
        // Act
        _sut.AddTransient<ITestService>(sp => new TestImplementation());

        // Assert
        _sut.Count.Should().Be(1);
        ServiceDescriptor descriptor = _sut.First();
        descriptor.ServiceType.Should().Be(typeof(ITestService));
        descriptor.ImplementationType.Should().Be(typeof(ITestService));
        descriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
        descriptor.ImplementationFactory.Should().NotBeNull();
    }
    
    [Fact]
    public void AddSingleton_With_Existing_Instance_Should_Add_ServiceDescriptor_To_ServiceCollection()
    {
        // Arrange
        var existingInstance = new TestImplementation();

        // Act
        _sut.AddSingleton(existingInstance);

        // Assert
        _sut.Count.Should().Be(1);
        ServiceDescriptor descriptor = _sut.First();
        descriptor.ServiceType.Should().Be(typeof(TestImplementation));
        descriptor.ImplementationType.Should().Be(typeof(TestImplementation));
        descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
        descriptor.Implementation.Should().Be(existingInstance);
    }
    
    [Fact]
    public void AddService_Should_Add_New_ServiceDescriptor_To_ServiceCollection()
    {
        // Arrange
        var newServiceDescriptor = new ServiceDescriptor();

        // Act
        _sut.AddService(newServiceDescriptor);

        // Assert
        _sut.Count.Should().Be(1);
        _sut.Should().Contain(newServiceDescriptor);
    }
}