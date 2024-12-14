using FluentAssertions;
using Markdown;

namespace MarkdownTests;

public class LinkTests
{
    private Md md;

    [SetUp]
    public void SetUp()
    {
        md = new Md();
    }
    [Test]
    public void Test_StandartLinks()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("[Link](https://link.com)").Should().Be("<a href=https://link.com>Link</a>");
        md.Render("[Link](https://example.com/?query=test&value=1)").Should().Be("<a href=https://example.com/?query=test&value=1>Link</a>");
        md.Render("[Тестовая ссылка](https://пример.рф)").Should().Be("<a href=https://пример.рф>Тестовая ссылка</a>");
        md.Render("[Click me](https://example.com/#anchor)").Should().Be("<a href=https://example.com/#anchor>Click me</a>");
    }

    [Test]
    public void Test_LinksWithoutName()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("[]()").Should().Be("");
        md.Render("[](aaa)").Should().Be("");
        md.Render("[     ](aaa)").Should().Be("");
        md.Render("(This is not a link)").Should().Be("(This is not a link)");
    }

    [Test]
    public void Test_LinksWithoutLink()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("[asd]()").Should().Be("<a href=>asd</a>");
        md.Render("[This is not a link]").Should().Be("[This is not a link]");
    }

    [Test]
    public void Test_LinksWithNestedBrackets()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("[Nes [ted [Link]](https://example.com)").Should().Be("[Nes <a href=https://example.com>ted [Link]</a>");
        md.Render("[Nested [Link]](https://example.com)").Should().Be("<a href=https://example.com>Nested [Link]</a>");
        md.Render("[Link with ()](https://example.com)").Should().Be("<a href=https://example.com>Link with ()</a>");
        md.Render("[Nested Link]](https://example.com)").Should().Be("[Nested Link]](https://example.com)");
    }

    [Test]
    public void Test_InvalidLinkFormatting()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("[UnclosedLink](https://example.com").Should().Be("[UnclosedLink](https://example.com");
        md.Render("Link](https://example.com)").Should().Be("Link](https://example.com)");
        md.Render("[Link]https://example.com").Should().Be("[Link]https://example.com");
    }

    [Test]
    public void Test_LinksWithNestedFormatting()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("[__Bold Link__](https://example.com)").Should().Be("<a href=https://example.com><strong>Bold Link</strong></a>");
        md.Render("[_Italic Link_](https://example.com)").Should().Be("<a href=https://example.com><em>Italic Link</em></a>");
        md.Render("[__Bold__ and _Italic_](https://example.com)").Should().Be("<a href=https://example.com><strong>Bold</strong> and <em>Italic</em></a>");
    }
}