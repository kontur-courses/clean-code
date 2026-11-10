using FluentAssertions;
using Markdown.Tokenization.Tokens.Implementations;

namespace MarkdownTests.HtmlBuilderTests;

public partial class HtmlBuilderTests
{
    [TestCase("[ссылка](url)", "<a href=\"url\">ссылка</a>")]
    [TestCase("[ссылка]()", "<a href=\"\">ссылка</a>")]
    [TestCase(
        "[A & B](https://example.org/?a=1&b=2)",
        "<a href=\"https://example.org/?a=1&amp;b=2\">A &amp; B</a>")]
    public void Append_WithLink_ShouldCreateHtmlAnchor(string source, string expected)
    {
        var urlStart = source.IndexOf("](") + 2;
        var urlLength = source.Length - urlStart - 1;

        var link = new LinkToken(
            source, 0, source.Length, urlStart, urlLength);

        Render(link).Should().Be(expected);
    }
}
