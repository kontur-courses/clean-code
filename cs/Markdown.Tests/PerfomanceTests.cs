using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown.Core.Lexing;
using Markdown.Core.Parsing;
using Markdown.Core.Rendering;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class PerformanceTests
{
    private Md _markdown;

    [SetUp]
    public void Setup()
    {
        var lexer = new Lexer();
        var parser = new Parser();
        var renderer = new Renderer();
        _markdown = new Md(lexer, parser, renderer);
    }

    [Test]
    public void Render_ShouldHandleLongInputLinearly()
    {
        const int paragraphs = 5000;

        var inputBuilder = new StringBuilder();
        var expectedBuilder = new StringBuilder();

        for (var i = 0; i < paragraphs; i++)
        {
            inputBuilder.Append($"__жирный__ {i} _курсив_  {i} текст");
            if (i < paragraphs - 1)
                inputBuilder.Append("\n\n");

            expectedBuilder.Append("<p><strong>жирный</strong> ")
                .Append(i)
                .Append(" <em>курсив</em>  ")
                .Append(i)
                .Append(" текст</p>");
        }

        var input = inputBuilder.ToString();
        var expected = expectedBuilder.ToString();

        var html = _markdown.Render(input);

        html.Should().Be(expected);
    }

    [Test]
    public void Render_ShouldScaleApproximatelyLinearlyWithInputSize()
    {
        const int smallParagraphs = 500;
        const int largeParagraphs = 5000;

        var smallInput = BuildRepeatedParagraphs(smallParagraphs, "__жирный__ _курсив_ текст");
        var largeInput = BuildRepeatedParagraphs(largeParagraphs, "__жирный__ _курсив_ текст");

        var smallDuration = MeasureMedianRenderMilliseconds(smallInput);
        var largeDuration = MeasureMedianRenderMilliseconds(largeInput);

        var baseline = Math.Max(1, smallDuration);
        largeDuration.Should().BeLessThanOrEqualTo(baseline * 25);
    }

    private long MeasureMedianRenderMilliseconds(string input)
    {
        const int runs = 3;
        var results = new long[runs];
        for (var i = 0; i < runs; i++)
        {
            var sw = Stopwatch.StartNew();
            _markdown.Render(input);
            sw.Stop();
            results[i] = sw.ElapsedMilliseconds;
        }
        Array.Sort(results);
        return results[runs / 2];
    }

    private static string BuildRepeatedParagraphs(int count, string paragraph)
    {
        var builder = new StringBuilder();

        for (var i = 0; i < count; i++)
        {
            builder.Append(paragraph);
            if (i < count - 1)
                builder.Append("\n\n");
        }
        return builder.ToString();
    }
    
}
