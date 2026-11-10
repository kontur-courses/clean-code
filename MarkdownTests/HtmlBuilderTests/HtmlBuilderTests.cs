using FluentAssertions;
using Markdown.Rendering;
using Markdown.Tokenization.Tokens;
using Markdown.Tokenization.Tokens.Abstractions;
using Markdown.Tokenization.Tokens.Implementations;

namespace MarkdownTests.HtmlBuilderTests;

public partial class HtmlBuilderTests
{
    [Test]
    public void Append_WithText_ShouldPreserveText()
    {
        var source = "Обычный текст";

        Render(new TextToken(source, 0, source.Length))
            .Should().Be(source);
    }

    [Test]
    public void Append_CalledMultipleTimes_ShouldAccumulateHtml()
    {
        var builder = new HtmlBuilder();

        builder.Append(new ResolvedTokens(
            [new TextToken("Hello", 0, 5)]));

        builder.Append(new ResolvedTokens(
            [new TextToken("World", 0, 5)]));

        builder.ToString().Should().Be("HelloWorld");
    }

    private static string Render(params Token[] tokens)
    {
        var builder = new HtmlBuilder();
        builder.Append(new ResolvedTokens(tokens));

        return builder.ToString();
    }
}
