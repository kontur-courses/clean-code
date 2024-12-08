using FluentAssertions;
using Markdown;

namespace MarkdownTests;

[TestFixture]
public class BoldTokenParserTests
{
    private BoldTokenParser parser;

    [SetUp]
    public void SetUp()
    {
        parser = new BoldTokenParser();
    }

    [TestCase("__a__", "a", TestName = "Simple text")]
    [TestCase("__w w__", "w w", TestName = "Several words")]
    [TestCase("__wor__d", "wor", TestName = "Bold in word")]
    public void ParserShouldParseCorrectText(string text, string expectedText)
    {
        var parseResult = parser.TryParseStringToToken(text);

        parseResult.HasToken.Should().BeTrue();
        parseResult.Token.Content.Should().Be(expectedText);
    }

    [TestCase("a__", TestName = "Text start not from start symbol")]
    [TestCase("__a_", TestName = "Another close tag")]
    [TestCase("__word wor_d", TestName = "Tags in another word")]
    [TestCase("__12__word", TestName = "Not tokenize digits")]
    [TestCase("__ a__", TestName = "Whitespace after start")]
    [TestCase("__a __", TestName = "Whitespace before end")]
    [TestCase("____", TestName = "Empty string")]
    public void ParserDontParseUncorrectText(string text)
    {
        var parseResult = parser.TryParseStringToToken(text);

        parseResult.HasToken.Should().BeFalse();
    }

    [Test]
    public void ParserShouldParseItalicInBoldText()
    {
        var text = "__bold _italic_ text__";
        var childs = new List<Token>()
        {
            new SimpleToken(TokenType.SimpleText, "bold "),
            new SimpleToken(TokenType.Italic, "italic"),
            new SimpleToken(TokenType.SimpleText, " text")
        };
        var expectedToken = new ComplexToken(TokenType.Bold, childs);

        var parseResult = parser.TryParseStringToToken(text);

        parseResult.HasToken.Should().BeTrue();
        parseResult.Token.Should().BeEquivalentTo(expectedToken);
    }
}
