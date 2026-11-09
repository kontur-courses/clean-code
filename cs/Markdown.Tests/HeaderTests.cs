using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class HeaderTests : MdTestBase
{
    [Test]
    public void Render_ParagraphStartingWithHashAndSpace_IsHeader()
        => Render("# Заголовок").Should().Be("<h1>Заголовок</h1>");

    [Test]
    public void Render_HeaderWithEmphasis_RendersEmphasisInside()
        => Render("# Заголовок __с _разными_ символами__")
            .Should().Be("<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>");

    [Test]
    public void Render_HeaderWithEscapedUnderscores_RemovesEscaping()
        => Render(@"# \_a\_").Should().Be("<h1>_a_</h1>");

    [Test]
    public void Render_HeaderWithUnpairedUnderscore_KeepsIt()
        => Render("# a_b").Should().Be("<h1>a_b</h1>");

    [TestCase("#Не заголовок")]
    [TestCase("текст # не заголовок")]
    [TestCase("  # отступ перед решёткой")]
    public void Render_NotAHeader_StaysAsIs(string input)
        => Render(input).Should().Be(input);

    [Test]
    public void Render_HashInsideLinkText_IsNotHeader()
        => Render("[# a](b)").Should().Be("<a href=\"b\"># a</a>");

    [Test]
    public void Render_HeaderInSecondParagraph_Works()
        => Render("текст\n\n# заголовок").Should().Be("текст\n\n<h1>заголовок</h1>");
}
