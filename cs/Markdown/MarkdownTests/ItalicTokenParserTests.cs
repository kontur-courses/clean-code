using FluentAssertions;
using Markdown;

namespace MarkdownTests;

[TestFixture]
public class ItalicTokenParserTests
{
    private ItalicTokenParser parser;

    [SetUp]
    public void SetUp()
    {
        parser = new ItalicTokenParser();
    }

    [TestCase("_a_", "a", TestName = "Simple text")]
    [TestCase("_word word_", "word word", TestName = "Text with several words")]
    [TestCase("_wor_d", "wor", TestName = "Italic in word")]
    [TestCase("_text __bold__ text_", "text __bold__ text", TestName = "Not tokenize bold in italic")]
    public void ParserCorrectParseText(string text, string expectedText)
    {
        var parseResult = parser.TryParseStringToToken(text);

        parseResult.HasToken.Should().BeTrue();
        var tokenText = parseResult.Token.Content;
        tokenText.Should().Be(expectedText);
    }

    [TestCase("a_", TestName = "Text start not from _")]
    [TestCase("_a__", TestName = "Another close tag")]
    [TestCase("_text te_xt", TestName = "Tags in another words")]
    [TestCase("_12_word", TestName = "Not tokenize digits")]
    [TestCase("_ a_", TestName = "Whitespace after start")]
    [TestCase("_a _", TestName = "Whitespace before end")]
    [TestCase("__", TestName = "Empty string")]
    public void ParserNotParseUncorrectText(string text)
    {
        var parceResult = parser.TryParseStringToToken(text);

        parceResult.HasToken.Should().BeFalse();
    }
}
