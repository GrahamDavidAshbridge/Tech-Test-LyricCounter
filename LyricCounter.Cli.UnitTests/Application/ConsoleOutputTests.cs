using System;
using System.IO;
using LyricCounter.Cli.Application;
using Shouldly;
using Xunit;

namespace LyricCounter.Cli.UnitTests.Application;

public class ConsoleOutputTests
{
    [Fact]
    public void ConsoleOutput_Should_WriteToConsole()
    {
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            var expected = "testy McTestface";
            var setupObject = new ConsoleOutput();
            setupObject.WriteLine(expected);
            sw.ToString().ShouldContain(expected);
        }
    }
}