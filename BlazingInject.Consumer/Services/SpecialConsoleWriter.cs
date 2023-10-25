namespace BlazingInject.Consumer;

public class SpecialConsoleWriter : IConsoleWriter
{
    public void WriteLine(string text)
    {
        Console.WriteLine($"Im special: {text}");
    }
}