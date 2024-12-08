using FluentAssertions;
using Markdown;

namespace MarkdownTests;

[TestFixture]
public class HeaderTokenParserTests
{
    private HeaderTokenParser parser;

    [SetUp]
    public void SetUp()
    {
        parser = new HeaderTokenParser();
    }

    [TestCase("# a\n", "a", TestName = "Simple text")]
    [TestCase("# word word\n", "word word", TestName = "Text with several words")]
    public void ParserCorrectParseText(string text, string expectedText)
    {
        var parseResult = parser.TryParseStringToToken(text);

        parseResult.HasToken.Should().BeTrue();
        var tokenText = parseResult.Token.Content;
        tokenText.Should().Be(expectedText);
    }

    [Test]
    public void ParserParseBoldInHeader()
    {
        var text = "# text __bold__ text\n";
        var childs = new List<Token>()
        {
            new SimpleToken(TokenType.SimpleText, "text "),
            new SimpleToken(TokenType.Bold, "bold"),
            new SimpleToken(TokenType.SimpleText, " text")
        };
        var expectedToken = new ComplexToken(TokenType.Header, childs);

        var parseResult = parser.TryParseStringToToken(text);

        parseResult.HasToken.Should().BeTrue();
        parseResult.Token.Should().BeEquivalentTo(expectedToken);
    }

    [Test]
    public void ParserParseItalicInHeader()
    {
        var text = "# text _italic_ text\n";
        var childs = new List<Token>()
        {
            new SimpleToken(TokenType.SimpleText, "text "),
            new SimpleToken(TokenType.Italic, "italic"),
            new SimpleToken(TokenType.SimpleText, " text")
        };
        var expectedToken = new ComplexToken(TokenType.Header, childs);

        var parseResult = parser.TryParseStringToToken(text);

        parseResult.HasToken.Should().BeTrue();
        parseResult.Token.Should().BeEquivalentTo(expectedToken);
    }

    [Test]
    public void ParserParseBoldAndItalicInHeader()
    {
        var text = "# text __bold _italic_ bold__ text\n";
        var childs = new List<Token>()
        {
            new SimpleToken(TokenType.SimpleText, "text "),
            new ComplexToken(TokenType.Bold, [
                new SimpleToken(TokenType.SimpleText, "bold "),
                new SimpleToken(TokenType.Italic, "italic"),
                new SimpleToken(TokenType.SimpleText, " bold")
                ]),
            new SimpleToken(TokenType.SimpleText, " text")
        };
        var expectedToken = new ComplexToken(TokenType.Header, childs);

        var parseResult = parser.TryParseStringToToken(text);

        parseResult.HasToken.Should().BeTrue();
        parseResult.Token.Should().BeEquivalentTo(expectedToken);
    }
}
