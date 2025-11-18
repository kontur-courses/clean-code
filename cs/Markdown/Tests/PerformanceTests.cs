using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown.Parsing;
using Markdown.Tokenizing;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class PerformanceTests
{
    private Md md = null!;

    [SetUp]
    public void SetUp()
        => md = new Md(new Lexer(), new Parser(), new HtmlRender());

    [Test]
    public void Renderer_RenderLargeTextFast()
    {
        var input = new string('a', 1000000);

        var sw = Stopwatch.StartNew();
        md.Render(input);
        sw.Stop();

        sw.ElapsedMilliseconds.Should().BeLessThan(50);
    }

    [Test]
    public void Renderer_RenderManyUnderscoresFast()
    {
        var input = new string('_', 20000);

        var sw = Stopwatch.StartNew();
        md.Render(input);
        sw.Stop();

        sw.ElapsedMilliseconds.Should().BeLessThan(30);
    }

    [Test]
    public void Renderer_RenderManyLinksFast()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < 5000; i++)
            sb.Append("[g](u) ");

        var input = sb.ToString();

        var sw = Stopwatch.StartNew();
        md.Render(input);
        sw.Stop();

        sw.ElapsedMilliseconds.Should().BeLessThan(40);
    }

    [Test]
    public void Renderer_RenderManyEscapesFast()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < 30_000; i++)
            sb.Append("\\_");

        var input = sb.ToString();

        var sw = Stopwatch.StartNew();
        md.Render(input);
        sw.Stop();

        sw.ElapsedMilliseconds.Should().BeLessThan(20);
    }

    [Test]
    public void Renderer_Scale_ApproximatelyLinearly()
    {
        var chunk = @"__text  _text  text_ text__ [x](y) \\text ";

        var inputSmall = string.Concat(Enumerable.Repeat(chunk, 10000));
        var inputLarge = string.Concat(Enumerable.Repeat(chunk, 20000));

        WarmupRenderer(md);

        var tSmall = MeasureAverageTicks(() => md.Render(inputSmall));
        var tLarge = MeasureAverageTicks(() => md.Render(inputLarge));

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
            md.Render("_x_");
    }
}