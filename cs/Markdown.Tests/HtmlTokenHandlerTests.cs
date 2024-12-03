using FluentAssertions;
using Markdown.TagInfo;
using Markdown.TokenInfo;
using Markdown.HtmlTools;

namespace Markdown.Tests;

public class HtmlTokenHandlerTests
{
    [Test]
    public void Handle_WithItalicToken()
    {
        var inputText = " _a_ ";
        var tokens = new List<Token>
        {
            new Token(Tag.Italic, 1, TagType.Open, 1), new Token(Tag.Italic, 3, TagType.Close, 1)
        };

        var result = HtmlTokenHandler.Handle(inputText, tokens);

        result.Should().Be(" <em>a</em> ");
    }

    [Test]
    public void Handle_WithStrongToken()
    {
        var inputText = "% __Hello__ c";
        var tokens = new List<Token>
        {
            new Token(Tag.Strong, 2, TagType.Open, 2), new Token(Tag.Strong, 9, TagType.Close, 2)
        };

        var result = HtmlTokenHandler.Handle(inputText, tokens);

        result.Should().Be("% <strong>Hello</strong> c");
    }

    [Test]
    public void Handle_WithHeaderToken()
    {
        var inputText = "# a";
        var tokens = new List<Token>
        {
            new Token(Tag.FirstLevelHeader, 0, TagType.Open, 2),
            new Token(Tag.FirstLevelHeader, 3, TagType.Close, 0)
        };

        var result = HtmlTokenHandler.Handle(inputText, tokens);

        result.Should().Be("<h1>a</h1>");
    }
}