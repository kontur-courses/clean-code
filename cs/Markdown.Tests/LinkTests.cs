using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class LinkTests : MdTestBase
{
    [TestCase("[текст](http://a.com)", "<a href=\"http://a.com\">текст</a>")]
    [TestCase("см. [тут](url) и [там](u2).", "см. <a href=\"url\">тут</a> и <a href=\"u2\">там</a>.")]
    [TestCase("[a](b)[c](d)", "<a href=\"b\">a</a><a href=\"d\">c</a>")]
    public void Render_Link_ProducesAnchor(string input, string expected)
        => Render(input).Should().Be(expected);

    [Test]
    public void Render_UnderscoresInUrl_AreNotEmphasis()
        => Render("[a](http://x.com/a_b_c) _d_")
            .Should().Be("<a href=\"http://x.com/a_b_c\">a</a> <em>d</em>");

    [Test]
    public void Render_EmphasisInsideLinkText_Works()
        => Render("[_a_ и __b__](url)")
            .Should().Be("<a href=\"url\"><em>a</em> и <strong>b</strong></a>");

    [Test]
    public void Render_NestedOpeningBracket_StartsNewLink()
        => Render("[a [b](c)").Should().Be("[a <a href=\"c\">b</a>");

    [TestCase("[a] (b)")]
    [TestCase("[a]")]
    [TestCase("[](b)")]
    [TestCase("[a]()")]
    [TestCase("[a](b c)")]
    [TestCase("[a](b")]
    [TestCase("[a](b\"c)")]
    [TestCase("[a](b(c))")]
    public void Render_NotALink_StaysAsIs(string input)
        => Render(input).Should().Be(input);

    [Test]
    public void Render_EscapedOpeningBracket_IsNotLink()
        => Render(@"\[a](b)").Should().Be("[a](b)");

    [Test]
    public void Render_EscapedClosingBracketInText_StaysInsideLink()
        => Render(@"[a\]b](c)").Should().Be("<a href=\"c\">a]b</a>");
}
