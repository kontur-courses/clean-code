using FluentAssertions;
using Markdown.Tokenization.Tokens.Implementations;

namespace MarkdownTests.HtmlBuilderTests;

public partial class HtmlBuilderTests
{
    [Test]
    public void Append_WithHtmlSymbols_ShouldEncodeThem()
    {
        var source = "<a> & \"hello\" 'world'";

        Render(new TextToken(source, 0, source.Length))
            .Should().Be("&lt;a&gt; &amp; &quot;hello&quot; &#39;world&#39;");
    }

    [Test]
    public void Append_WithEscapedToken_ShouldRemoveBackslash()
    {
        var source = @"\_текст";

        Render(
            new EscapedToken(source, 0),
            new TextToken(source, 2, 5))
            .Should().Be("_текст");
    }
}
