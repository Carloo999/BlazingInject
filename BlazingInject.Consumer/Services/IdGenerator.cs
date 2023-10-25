namespace BlazingInject.Consumer;

public class IdGenerator : IIdGenerator
{
    public Guid Id { get; } = Guid.NewGuid();
    private readonly IConsoleWriter _consoleWriter;

    public IdGenerator(IConsoleWriter consoleWriter)
    {
        _consoleWriter = consoleWriter;
    }

    public void PrintGuid()
    {
        _consoleWriter.WriteLine(Id.ToString());
    }
}