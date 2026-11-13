using System.Diagnostics;
using FluentAssertions;
using Markdown;
using Markdown.Parsers;
using Markdown.Renderers;

namespace MarkdownUnitTests;

[TestFixture]
public class MdPerformanceTests
{
    private static readonly Md Md = new Md(new MarkdownParser(), new HtmlRenderer());
    
    [TestCase("обычный текст ",
        TestName = "Render_PlainText_TimeGrowsAlmostLinearly")]
    [TestCase("_a ",
        TestName = "Render_UnmatchedMarkers_TimeGrowsAlmostLinearly")]
    [TestCase("# a\n ",
        TestName = "Render_Heading_TimeGrowsAlmostLinearly")]
    [TestCase("_a_ ",
        TestName = "Render_Emphasis_TimeGrowsAlmostLinearly")]
    [TestCase("__a__ ",
        TestName = "Render_Strong_TimeGrowsAlmostLinearly")]
    [TestCase("\\a ",
        TestName = "Render_Escaping_TimeGrowsAlmostLinearly")]
    public void Render_TimeGrowsAlmostLinearly(string fragment)
    {
        var smallInput = Repeat(fragment, 10_000);
        var largeInput = Repeat(fragment, 100_000);

        // Прогрев: уменьшаем влияние первого запуска и JIT-компиляции.
        Md.Render(smallInput);
        Md.Render(largeInput);

        var smallTime = MeasureMedianTime(smallInput);
        var largeTime = MeasureMedianTime(largeInput);

        TestContext.Out.WriteLine(
            $"Малый вход: {smallInput.Length} символов, " +
            $"{smallTime.TotalMilliseconds:F2} мс");

        TestContext.Out.WriteLine(
            $"Большой вход: {largeInput.Length} символов, " +
            $"{largeTime.TotalMilliseconds:F2} мс");

        // Запас относительно ожидаемого роста в 10 раз.
        // Дополнительные 20 мс компенсируют шум на быстрых запусках.
        var inputGrowthFactor = (double)largeInput.Length / smallInput.Length;
        var measurementTolerance = TimeSpan.FromMilliseconds(20);
        
        var allowedTime = TimeSpan.FromMilliseconds(
            smallTime.TotalMilliseconds * inputGrowthFactor) 
                          + measurementTolerance;

        largeTime.Should().BeLessThan(
            allowedTime,
            "при увеличении входа в {0} раз время не должно расти квадратично",
            inputGrowthFactor);
    }

    private static TimeSpan MeasureMedianTime(string input)
    {
        var measurements = new long[5];

        for (var i = 0; i < measurements.Length; i++)
        {
            var stopwatch = Stopwatch.StartNew();

            var result = Md.Render(input);

            stopwatch.Stop();
            measurements[i] = stopwatch.ElapsedTicks;
        }

        Array.Sort(measurements);

        return TimeSpan.FromSeconds(
            (double)measurements[measurements.Length / 2]
            / Stopwatch.Frequency);
    }

    private static string Repeat(string fragment, int count)
        => string.Concat(Enumerable.Repeat(fragment, count));
}