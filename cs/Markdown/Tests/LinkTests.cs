using System.Diagnostics;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class LinkTests
{
    private Md md = null!;
    
    private static Md CreateMd() => new(
        new Tokenizer(
            new TagReaders.TagReadersProvider(
            [
                new TagReaders.TextReader(),
                new TagReaders.LinkTagReader(),
                new TagReaders.UnderscoresTagsReader(),
                new TagReaders.HeaderTagReader()
            ])));
    
    [SetUp]
    public void SetUp() => md = CreateMd();

    [TestCase("[текст](http://a.com)", "<a href=\"http://a.com\">текст</a>")]
    [TestCase("см. [тут](url) и [там](u2).", "см. <a href=\"url\">тут</a> и <a href=\"u2\">там</a>.")]
    [TestCase("# Заголовок [a](b)", "<h1>Заголовок <a href=\"b\">a</a></h1>")]
    [TestCase("[a [b](c)", "[a <a href=\"c\">b</a>")]
    public void Render_Link(string input, string expected)
        => Assert.That(md.Render(input), Is.EqualTo(expected));

    [TestCase("[_a_ и __b__](url)", "<a href=\"url\"><em>a</em> и <strong>b</strong></a>")]
    [TestCase("_[a](b)_", "<em><a href=\"b\">a</a></em>")]
    [TestCase("__x [a](b) y__", "<strong>x <a href=\"b\">a</a> y</strong>")]
    [TestCase("[a](http://x.com/a_b_c) _d_", "<a href=\"http://x.com/a_b_c\">a</a> <em>d</em>")]
    [TestCase("_a [b_](c)", "_a <a href=\"c\">b_</a>")]
    public void Render_LinkAndEmphasis(string input, string expected)
        => Assert.That(md.Render(input), Is.EqualTo(expected));

    [TestCase(@"\[a](b)", "[a](b)")]
    [TestCase(@"[a\]b](c)", "<a href=\"c\">a]b</a>")]
    public void Render_LinkEscaping(string input, string expected)
        => Assert.That(md.Render(input), Is.EqualTo(expected));

    [TestCase("[a] (b)")]
    [TestCase("[a]")]
    [TestCase("[](b)")]
    [TestCase("[a]()")]
    [TestCase("[a](b c)")]
    [TestCase("[a](b")]
    [TestCase("[a](b\"c)")]
    [TestCase("[a](b(c))")]
    public void Render_NotALink_StaysAsIs(string input)
        => Assert.That(md.Render(input), Is.EqualTo(input));

    [TestCase("[a](b) ")]
    [TestCase("[[a](b")]
    [TestCase("[a](x[a](x")]
    [TestCase("[_a_ __b ")]
    public void Render_Links_WorkInLinearTime(string unit)
    {
        md.Render(string.Concat(Enumerable.Repeat(unit, 1000))); 

        var smallTime = Measure(unit, 50_000);
        var largeTime = Measure(unit, 500_000);

        Assert.That(largeTime, Is.LessThan(Math.Max(smallTime * 30, 200)),
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
