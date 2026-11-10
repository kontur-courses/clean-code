using System.Diagnostics;
using FluentAssertions;

namespace Markdown.Tests;

[TestFixture]
public class MarkdownPerformanceTests
{
    [Test]
    [Explicit("Performance test")]
    public void ToHtml_DoubleInputSize_TimeShouldGrowLinearly()
    {
        const string text = """
                            # Header
                            
                            paragraph with __strong__ _emphasis_
                            ___three___
                            ___EM_ strong__
                            \\_escaped\_ \_emphasis\_
                            
                            just very large very big very giant text lalalalalal
                            ops.... its same paragraph
                            
                            
                            # Ordered lists
                            
                            - one
                            - two
                            - three
                            - four
                            - five
                            
                            
                            - again
                            - try
                            - no
                            
                            random delimiters _asd_ __asdf___ASdf _ asdf_ __asdf _ asdf__ asdf_ asdf_a a___ asdf___
                            
                            в нач_але, и в сер_еди_не, и в кон_це.
                            
                            
                            цифры_1_2_3_
                            """;

        var small = string.Concat(
            Enumerable.Repeat(text, 10_000));

        var large = string.Concat(
            Enumerable.Repeat(text, 20_000));

        // Прогрев JIT
        MarkdownConverter.ToHtml("Hello");

        var smallTime = Measure(small);
        var largeTime = Measure(large);

        var ratio = largeTime / smallTime;

        TestContext.Out.WriteLine($"Small: {smallTime:F2} ms");
        TestContext.Out.WriteLine($"Large: {largeTime:F2} ms");
        TestContext.Out.WriteLine($"Ratio: {ratio:F2}");

        ratio.Should().BeLessThan(3.0);
    }

    private static double Measure(string markdown)
    {
        const int iterations = 20;
        var times = new List<double>();

        for (var i = 0; i < iterations; i++)
        {
            var sw = Stopwatch.StartNew();

            _ = MarkdownConverter.ToHtml(markdown);

            sw.Stop();
            times.Add(sw.Elapsed.TotalMilliseconds);
        }

        times.Sort();
        return times[iterations / 2];
    }
}