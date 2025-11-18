using FluentAssertions;
using Markdown;
using Markdown.Tags;
using System.Diagnostics;

namespace MarkdownTests;

[TestFixture]
public class MdPerformanceTests
{
    private Md mdRenderer;

    [OneTimeSetUp]
    public void Setup()
    {
        var supportedTags = new List<ITag>
            { new HeaderTag(), new ItalicTag(), new StrongTag() };
        mdRenderer = new Md(supportedTags);
    }

    [Test]
    public void Render_PerformanceTest_ShouldHaveSubQuadraticComplexity()
    {
        var pattern = "# Заголовок с _курсивным_ текстом и __жирным__ выделением\n";
        pattern += "А это второй абзац с разными _символами_ разметки и \\_экранированием\\_\n";
        pattern += "И еще один __абзац__ для _тестирования_ производительности # не заголовок";
        pattern += "* И еще блок из * Списка 1 \n * A \n и \n * Списка 2\n * Б \n ";

        var smallText = string.Join("", Enumerable.Repeat(pattern, 100));
        var largeText = string.Join("", Enumerable.Repeat(pattern, 1000));

        var smallStopwatch = Stopwatch.StartNew();
        var smallResult = mdRenderer.Render(smallText);
        smallStopwatch.Stop();
        var smallTime = smallStopwatch.ElapsedMilliseconds;

        var largeStopwatch = Stopwatch.StartNew();
        var largeResult = mdRenderer.Render(largeText);
        largeStopwatch.Stop();
        var largeTime = largeStopwatch.ElapsedMilliseconds;

        smallResult.Should().NotBeNull();
        largeResult.Should().NotBeNull();

        var ratio = (double)largeTime / smallTime;
        ratio.Should().BeLessThan(40);

        TestContext.Out.WriteLine($"Small text {smallText.Length} chars: {smallTime}ms");
        TestContext.Out.WriteLine($"Large text {largeText.Length} chars: {largeTime}ms");
        TestContext.Out.WriteLine($"Ratio: {ratio:F2}");
    }
}