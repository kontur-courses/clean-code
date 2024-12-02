using System.Diagnostics;
using System.Text;
using FluentAssertions;

namespace Markdown.Tests;

[TestFixture]
[TestOf(typeof(Md))]
public partial class MdTests
{
    private readonly Md markdownConverter = new();

    [Test]
    [TestCaseSource(nameof(conversionTestCases))]
    [TestCaseSource(nameof(noConversionTestCases))]
    public void Render_ShouldCorrectlyConvert_VariousInputs(string text, string expected)
    {
        var convertResult = markdownConverter.Render(text);
        convertResult.Should().BeEquivalentTo(expected);
    }

    [Test]
    [TestCase(1000)]
    public void Render_Should_LinearComplexity(int size)
    {
        const int baseSize = 100;
        const int iterations = 200;
        var baseMarkdownText = GenerateMarkdownText(baseSize);
        var markdownText = GenerateMarkdownText(size);
        var expectedTime = GetTimeForChar(baseMarkdownText, iterations);
        var actualTime = GetTimeForChar(markdownText, iterations);
        var deviationTime = expectedTime * 0.5;

        Assert.That(actualTime, Is.EqualTo(expectedTime).Within(deviationTime));
    }

    private TimeSpan GetTimeForChar(string text, int count)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        var stopwatch = Stopwatch.StartNew();
        for (var i = 0; i < count; i++)
        {
            markdownConverter.Render(text);
        }
        stopwatch.Stop();
        return stopwatch.Elapsed / count / text.Length;
    }

    private string GenerateMarkdownText(int size)
    {
        var random = new Random();
        var builder = new StringBuilder();
        var headerCount = random.Next(1, size / 100 + 1);
        var paragraphCount = random.Next(size / 50, size / 10 + 1);
        for (var i = 0; i < headerCount; i++)
        {
            builder.AppendLine($"# Заголовок {i} с _курсивом_ и __полужирным__ и \\_экранированным_ текстом");
        }
        for (var i = 0; i < paragraphCount; i++)
        {
            var wordCount = random.Next(10, 50);
            for (var j = 0; j < wordCount; j++)
            {
                if (random.NextDouble() < 0.2)
                    builder.Append("_слово_ ");
                else if (random.NextDouble() < 0.2)
                    builder.Append("__слово__ ");
                else if (random.NextDouble() < 0.2)
                    builder.Append("\\_слово\\_ ");
                else
                    builder.Append("слово" + " ");
            }

            builder.AppendLine();
        }

        var remaining = size - builder.Length;
        if (remaining > 0)
        {
            builder.Append(new string('а', remaining));
        }
        return builder.ToString();
    }
}