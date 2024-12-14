using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown;
using Markdown.Converters;
using Markdown.MarkdownTagValidators;
using Markdown.Parsers.MarkdownLineParsers;
using Markdown.Parsers.TokensParsers;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class MarkdownRenderTests
{
    private const double ExpectedExecutionTimeOnSymbolInMilliseconds = 0.0025;
    private readonly MarkdownLineParser mdLineParser = new();
    private readonly HtmlConverter htmlConverter = new();
    private readonly TokensParser tokenParser = new(new MarkdownTagValidator());
    private Md md;
    
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        md = new Md(mdLineParser, htmlConverter, tokenParser);
    }
    
    [TestCase(100)]
    [TestCase(500)]
    public void Render_ShouldWorkOnLineal(int numberOfRepetitions)
    {
        var markdownSpecText = File.ReadAllText("MdTestForTest.md");
        var text = RepeatText(markdownSpecText);

        var actualExecutionTime = GetExecutionTimeInMilliseconds(() => md.Render(text));
        var executionTimeLimit = ExpectedExecutionTimeOnSymbolInMilliseconds * text.Length;

        Console.WriteLine(actualExecutionTime);
        Console.WriteLine(executionTimeLimit);
        
        actualExecutionTime.Should().BeLessThanOrEqualTo(executionTimeLimit);
    }
    
    private static string RepeatText(string text, int count = 1)
    {
        var textBuilder = new StringBuilder();
        for (var i = 0; i < count; i++)
            textBuilder.AppendLine(text);

        return textBuilder.ToString();
    }
    
    private static double GetExecutionTimeInMilliseconds(Action action)
    {
        var stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();

        return stopwatch.ElapsedMilliseconds;
    }
}