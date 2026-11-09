using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MixedTests : MdTestBase
{
    [Test]
    public void Render_HeaderWithEmphasisAndLink()
        => Render("# Заголовок _с_ [ссылкой](url)")
            .Should().Be("<h1>Заголовок <em>с</em> <a href=\"url\">ссылкой</a></h1>");

    [Test]
    public void Render_HeaderWithLinkContainingEmphasis()
        => Render("# [__a__ и _b_](url)")
            .Should().Be("<h1><a href=\"url\"><strong>a</strong> и <em>b</em></a></h1>");

    [Test]
    public void Render_EmphasisAroundLink()
        => Render("_[a](b)_ и __[c](d)__")
            .Should().Be("<em><a href=\"b\">a</a></em> и <strong><a href=\"d\">c</a></strong>");

    [Test]
    public void Render_EmphasisSpanningTextAndLink()
        => Render("_a [b](c) d_").Should().Be("<em>a <a href=\"c\">b</a> d</em>");

    [Test]
    public void Render_UnderscoreInsideLinkAndOutside_DoNotPair()
        => Render("_a [b_](c)").Should().Be("_a <a href=\"c\">b_</a>");

    [Test]
    public void Render_UnpairedUnderscoreInLinkText_DoesNotAffectOutsideEmphasis()
        => Render("_a_ [b_](c)").Should().Be("<em>a</em> <a href=\"c\">b_</a>");

    [Test]
    public void Render_LinkTextWithUnpairedOpeningUnderscore_ClosingOutsideStaysText()
        => Render("[_a](b)_").Should().Be("<a href=\"b\">_a</a>_");

    [Test]
    public void Render_EscapedUnderscoresNextToLink()
        => Render(@"\_не выделение\_ и [a](b)")
            .Should().Be("_не выделение_ и <a href=\"b\">a</a>");

    [Test]
    public void Render_FullDocument()
        => Render("# Заголовок\n\nТекст с _курсивом_ и [ссылкой](u).\n\n__жирный__")
            .Should().Be("<h1>Заголовок</h1>\n\nТекст с <em>курсивом</em> и <a href=\"u\">ссылкой</a>.\n\n<strong>жирный</strong>");
}
