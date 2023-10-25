namespace BlazingInject.Consumer;

public interface IIdGenerator
{
    public Guid Id { get; }
    public void PrintGuid();
}