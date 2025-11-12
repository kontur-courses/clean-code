using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class PerformanceTests
{
    private Md md = null!;

    [SetUp]
    public void SetUp()
        => md = new Md();

    [Test]
    public void Renderer_ShouldRenderLargeTextFast_Test()
    {
        var input = new string('a', 1_000_000);

        var sw = Stopwatch.StartNew();
        Md.Render(input);
        sw.Stop();

        sw.ElapsedMilliseconds.Should().BeLessThan(50);
    }

    [Test]
    public void Renderer_ShouldRenderManyUnderscoresFast_Test()
    {
        var input = new string('_', 20_000);

        var sw = Stopwatch.StartNew();
        Md.Render(input);
        sw.Stop();

        sw.ElapsedMilliseconds.Should().BeLessThan(20);
    }

    [Test]
    public void Renderer_ShouldRenderManyLinksFast_Test()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < 5000; i++)
            sb.Append("[g](u) ");

        var input = sb.ToString();

        var sw = Stopwatch.StartNew();
        Md.Render(input);
        sw.Stop();

        sw.ElapsedMilliseconds.Should().BeLessThan(40);
    }

    [Test]
    public void Renderer_ShouldRenderManyEscapesFast_Test()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < 30_000; i++)
            sb.Append("\\_");

        var input = sb.ToString();

        var sw = Stopwatch.StartNew();
        Md.Render(input);
        sw.Stop();

        sw.ElapsedMilliseconds.Should().BeLessThan(30);
    }

    [Test]
    public void Renderer_ShouldScale_ApproximatelyLinearly_Test()
    {
        var chunk = @"__a _b c_ d__ [x](y) \\text ";

        var inputSmall = string.Concat(Enumerable.Repeat(chunk, 2000));
        var inputLarge = string.Concat(Enumerable.Repeat(chunk, 4000));

        WarmupRenderer(md);

        var tSmall = MeasureAverageTicks(() => Md.Render(inputSmall));
        var tLarge = MeasureAverageTicks(() => Md.Render(inputLarge));

        var ratio = (double)tLarge / tSmall;

        TestContext.WriteLine($"tSmall={tSmall}, tLarge={tLarge}, ratio={ratio}");

        ratio.Should().BeInRange(1.4, 2.7);
    }

    private static long MeasureAverageTicks(Action action)
    {
        var sw = new Stopwatch();
        var sum = 0L;

        for (var i = 0; i < 5; i++)
        {
            sw.Restart();
            action();
            sw.Stop();
            sum += sw.ElapsedTicks;
        }

        return sum / 5;
    }

    private static void WarmupRenderer(Md md)
    {
        for (var i = 0; i < 10; i++)
            Md.Render("_x_");
    }
}