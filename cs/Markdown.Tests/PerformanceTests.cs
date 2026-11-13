using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class PerformanceTests : MdTestBase
{
    private const int SmallRepeats = 50_000;
    private const int LargeRepeats = 500_000;
    
    [TestCase("_a ")]
    [TestCase("_a __b ")]
    [TestCase("_a__b_ __c__ ")]
    [TestCase("[a](b) ")]
    [TestCase("[[a](b")]
    [TestCase("[a](x[a](x")]
    [TestCase("[_a_ __b ")]
    [TestCase("a _b_\n\n")]
    public void Render_WorksInLinearTime(string unit)
    {
        md.Render(string.Concat(Enumerable.Repeat(unit, 1000))); 

        var smallTime = Measure(unit, SmallRepeats);
        var largeTime = Measure(unit, LargeRepeats);
        
        largeTime.Should().BeLessThan(Math.Max(smallTime * 30, 200),
            $"small={smallTime}ms, large={largeTime}ms");
    }

    private long Measure(string unit, int repeats)
    {
        var input = string.Concat(Enumerable.Repeat(unit, repeats));
        var watch = Stopwatch.StartNew();
        md.Render(input);
        return watch.ElapsedMilliseconds;
    }
}
