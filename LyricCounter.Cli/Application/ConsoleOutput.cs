namespace LyricCounter.Cli.Application
{
    public class ConsoleOutput : IConsoleOutput
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}