using System.Diagnostics;
using FluentAssertions;
using Markdown;
using Markdown.TagConverter;
using Markdown.Parser;
using Markdown.Syntax;

namespace MarkdownTests;

public class MarkupRendererTests
{
    private MarkupRenderer sut;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        sut = new MarkupRenderer(new AnySyntaxParser(new MarkdownToTokenSyntax()),
            new MarkupConverter(new TokenToHtmlSyntax()));
    }

    [TestCaseSource(typeof(MarkupRendererTestCases), nameof(MarkupRendererTestCases.RenderTestCases))]
    public void MarkupRenderer_Should(string input, string expectedString)
    {
        var renderedString = sut.Render(input);

        renderedString.Should().Be(expectedString);
    }

    [Test]
    public void MarkupRenderer_ShouldHaveLinearComplexity()
    {
        var repetitionsCount = 1000;
        var inputString = "![aba](caba) Text___with_different__tags\\__";

        var shortString = string.Concat(Enumerable.Repeat(inputString, repetitionsCount));
        var longString = string.Concat(Enumerable.Repeat(inputString, repetitionsCount * 100));

        var stopwatch = new Stopwatch();
        sut.Render(inputString);
        
        GC.Collect();
        GC.WaitForPendingFinalizers();
        stopwatch.Start();
        sut.Render(shortString);
        stopwatch.Stop();

        var shortStringTime = stopwatch.ElapsedMilliseconds;

        GC.Collect();
        GC.WaitForPendingFinalizers();
        stopwatch.Start();
        sut.Render(longString);
        stopwatch.Stop();

        var longStringTime = stopwatch.ElapsedMilliseconds;

        var timeRatio = longStringTime / shortStringTime;

        timeRatio.Should().BeLessOrEqualTo(100 * 2);
    }
}