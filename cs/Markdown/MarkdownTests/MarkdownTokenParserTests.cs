using FluentAssertions;
using Markdown;

namespace MarkdownTests;

[TestFixture]
public class MarkdownTokenParserTests
{
    private MarkdownParser parser;

    [SetUp]
    public void SetUp()
    {
        parser = new MarkdownParser([
            new BoldTokenParser(),
            new ItalicTokenParser(),
            new HeaderTokenParser(),
            new LinkTokenParser(),
            ]);
    }

    [Test]
    public void Parser_ParseSimpleText_WithTokens()
    {
        var text = "# header\nsimple text _italic_ __bold__";
        var expectedTokens = new List<Token>()
        {
            new SimpleToken(TokenType.Header, "header"),
            new SimpleToken(TokenType.SimpleText, "simple text "),
            new SimpleToken(TokenType.Italic, "italic"),
            new SimpleToken(TokenType.SimpleText, " "),
            new SimpleToken(TokenType.Bold, "bold")
        };

        var tokens = parser.ParseTextToTokens(text);

        tokens.Should().BeEquivalentTo(expectedTokens);
    }

    [Test]
    public void Parser_ShouldParse_WithComplexToken()
    {
        var text = "# header _italic_\n text __bold _italic_ bold__";
        var expectedTokens = new List<Token>()
        {
            new ComplexToken(TokenType.Header, [
                new SimpleToken(TokenType.SimpleText, "header "),
                new SimpleToken(TokenType.Italic, "italic")
                ]),
            new SimpleToken(TokenType.SimpleText, " text "),
            new ComplexToken(TokenType.Bold, [
                new SimpleToken(TokenType.SimpleText, "bold "),
                new SimpleToken(TokenType.Italic, "italic"),
                new SimpleToken(TokenType.SimpleText, " bold")
                ])
        };

        var tokens = parser.ParseTextToTokens(text);

        tokens.Should().BeEquivalentTo(expectedTokens);
    }

    [Test]
    public void Parser_ShouldSkipEscapedSymbols_InStart()
    {
        var text = "text \\_italic_";
        var expectedTokens = new List<Token>()
        {
            new SimpleToken(TokenType.SimpleText, text)
        };

        var tokens = parser.ParseTextToTokens(text);

        tokens.Should().BeEquivalentTo(expectedTokens);
    }

    [Test]
    public void Parser_ShouldSkipEscapedSymbols_InEnd()
    {
        var text = "text _italic\\_ text";
        var expectedTokens = new List<Token>()
        {
            new SimpleToken(TokenType.SimpleText, text)
        };

        var tokens = parser.ParseTextToTokens(text);

        tokens.Should().BeEquivalentTo(expectedTokens);
    }

    [Test]
    public void Parser_ShouldEscape_EscapeSymbol()
    {
        var text = "text \\\\_italic_ text";
        var expectedTokens = new List<Token>()
        {
            new SimpleToken(TokenType.SimpleText, "text \\\\"),
            new SimpleToken(TokenType.Italic, "italic"),
            new SimpleToken(TokenType.SimpleText, " text")
        };

        var tokens = parser.ParseTextToTokens(text);

        tokens.Should().BeEquivalentTo(expectedTokens);
    }

    [Test]
    public void Parser_ParseTextWithLink()
    {
        var text = "_italic_ __bold__ [[link](href)";
        var expectedTokens = new List<Token>()
        {
            new SimpleToken(TokenType.Italic, "italic"),
            new SimpleToken(TokenType.SimpleText, " "),
            new SimpleToken(TokenType.Bold, "bold"),
            new SimpleToken(TokenType.SimpleText, " ["),
            new TokenWithArgument(TokenType.Link, "link", "href")
        };

        var actualTokens = parser.ParseTextToTokens(text);

        actualTokens.Should().BeEquivalentTo(expectedTokens);
    }
}
