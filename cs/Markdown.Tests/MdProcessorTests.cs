using System.Diagnostics;
using System.Text;
using Markdown;
using FluentAssertions;

namespace MarkdownTests;

[TestFixture]
public class MdProcessorTests
{
    [TestCase(@"", @"<br/>", TestName = "Пустая строка", Category = "br")]
    [TestCase(
        @"It's hard to be a _Washington Capitals_ fan.",
        "It's hard to be a <em>Washington Capitals</em> fan.<br/>",
        TestName = "Одинарное подчеркивание. Курсив.", Category = "<em>")]
    [TestCase(
        @"It's hard to be a __Washington Capitals__ fan.",
        @"It's hard to be a <strong>Washington Capitals</strong> fan.<br/>",
        TestName = "Двойное подчеркивание. Жирный.", Category = "<strong>")]
    [TestCase(
        @"It's hard to be a \_Washington Capitals\_ fan.",
        @"It's hard to be a _Washington Capitals_ fan.<br/>",
        TestName = "Экранирование символом \\", Category = "\\")]
    [TestCase(
        @"It's hard to be a \Washington Capitals\ fan.",
        @"It's hard to be a \Washington Capitals\ fan.<br/>",
        TestName = "\\ Без экранирования символов", Category = "\\")]
    [TestCase(
        @"It's hard to be a \\_Washington Capitals_ fan.",
        @"It's hard to be a \<em>Washington Capitals</em> fan.<br/>",
        TestName = "Двойное экранирование", Category = "\\")]
    [TestCase(@"It's __hard to be a _Washington Capitals_ fan.__",
        @"It's <strong>hard to be a <em>Washington Capitals</em> fan.</strong><br/>",
        TestName = "Жирный поверх курсива")]
    [TestCase(
        @"It's _hard to be a __Washington Capitals__ fan._",
        @"It's <em>hard to be a __Washington Capitals__ fan.</em><br/>",
        TestName = "Внутри курсива жирный не работает")]
    [TestCase(
        @"It's hard to be a Washington Capitals fan in _202_3.",
        @"It's hard to be a Washington Capitals fan in _202_3.<br/>",
        TestName = "Выделение частей чисел - не работает")]
    [TestCase(
        @"It's hard to be a Washington ____ Capitals fan.",
        @"It's hard to be a Washington ____ Capitals fan.<br/>",
        TestName = "Отсутствие символов между тегами")]
    [TestCase(
        @"# It's __hard to be a _Washington Capitals_ fan.__",
        @"<h1>It's <strong>hard to be a <em>Washington Capitals</em> fan.</strong></h1><br/>",
        TestName = "Заголовок с тегами внутри", Category = "<h1>")]
    public void Rendering_ShouldOutputCorrectHtml(string inputText, string expectedHtml)
    {
        var actualHtml = MdProcessor.Render(inputText);
        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Rendering_WithLinearTimeComplexity()
    {
        var initialText = @"# It's __hard__ to be a \_Washington Capitals_\ fan.,\n
                       It's _hard_ to be a _Washington Capitals_ fan.,\n
                       It's hard to be a _Washington Capitals_ fan.,\n
                       #It's __hard__ to be a \_Washington Capitals_\ fan.\n
                       It's hard to be a _Washington Capitals_ fan.\n
                       It's \hard\ to be a _Washington Capitals_ fan.\n
                       # It's hard to be a \_Washington Capitals_\ fan.\n
                       It's \\hard to be a \\_Washington Capitals_\\ fan.\n
                       It's _hard_ to be a _Washington Capitals_ fan.\n
                       It's ____hard to be a _Washington Capitals_ fan.\n
                       #It's hard to be a _Washington Capitals_ fan.\n
                       It's __hard to be a _Washington Capitals_ fan.__\n";
        
        const int measurementsCount = 1_000;
        const int textExpansionFactor = 5;
        const int tolerance = 1;

        var initialTime = MeasureRenderingTime(initialText, measurementsCount);

        var expandedText = ExpandText(initialText, textExpansionFactor);
        var expandedTime = MeasureRenderingTime(expandedText, measurementsCount);

        var expectedTime = initialTime * textExpansionFactor;
        var difference = Math.Abs(expandedTime - expectedTime);

        difference.Should().BeLessThanOrEqualTo(tolerance);
    }

    private double MeasureRenderingTime(string text, int measuresCount)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        for (var i = 0; i < measuresCount; i++)
            MdProcessor.Render(text);

        stopWatch.Stop();

        return stopWatch.Elapsed.TotalMilliseconds / measuresCount;
    }

    private string ExpandText(string text, int times)
    {
        StringBuilder expandedText = new StringBuilder(text);

        for (int i = 0; i < times - 1; i++)
        {
            expandedText.Append(text);
        }

        return expandedText.ToString();
    }
}