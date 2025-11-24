using FluentAssertions;
using Markdown.Domains;
using Markdown.Lexer;

namespace MarkdownTest.Lexer;

public class Tests
{
    private static IEnumerable<TestCaseData> LexerTokenizeTestCases()
    {
        yield return new TestCaseData(
            "#Test",
            new List<MdToken>
            {
                new(TokenType.Grid, "#"),
                new(TokenType.Word, "Test")
            }
        ).SetName("WithGridAndWord");

        yield return new TestCaseData(
            "_Test",
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "Test")
            }
        ).SetName("WithUnderscoreAndWord");

        yield return new TestCaseData(
            "_123",
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Number, "123")
            }
        ).SetName("WithUnderscoreAndNumber");

        yield return new TestCaseData(
            "123Test",
            new List<MdToken>
            {
                new(TokenType.Number, "123"),
                new(TokenType.Word, "Test")
            }
        ).SetName("WithWordAndNumber");

        yield return new TestCaseData(
            "TE_ST",
            new List<MdToken>
            {
                new(TokenType.Word, "TE"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "ST")
            }
        ).SetName("WithWordAndUnderscoreInside");

        yield return new TestCaseData(
            "_Test_ #Text 123",
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "Test"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Space, " "),
                new(TokenType.Grid, "#"),
                new(TokenType.Word, "Text"),
                new(TokenType.Space, " "),
                new(TokenType.Number, "123")
            }
        ).SetName("WithAllTokens");

        yield return new TestCaseData(
            "[text](example.com)",
            new List<MdToken>
            {
                new(TokenType.LeftSquareBracket, "["),
                new(TokenType.Word, "text"),
                new(TokenType.RightSquareBracket, "]"),
                new(TokenType.LeftParenthesis, "("),
                new(TokenType.Word, "example.com"),
                new(TokenType.RightParenthesis, ")")
            }
        ).SetName("WithSquareBracketAndParenthesis");
    }

    [Test]
    [TestCaseSource(nameof(LexerTokenizeTestCases))]
    public void Tokenize_ShouldTokenize_Correctly(string input, List<MdToken> expectedOutput)
    {
        var actualInput = MdLexer.Tokenize(input);

        actualInput.Should().BeEquivalentTo(expectedOutput);
    }

    [Test]
    [TestCase('T', TokenType.Word)]
    [TestCase('a', TokenType.Word)]
    [TestCase('#', TokenType.Grid)]
    [TestCase('_', TokenType.Underscore)]
    [TestCase(' ', TokenType.Space)]
    [TestCase('\u00a0', TokenType.Space)]
    [TestCase('\u200b', TokenType.Space)]
    [TestCase('\t', TokenType.Space)]
    [TestCase('\n', TokenType.Escape)]
    [TestCase('\r', TokenType.Escape)]
    [TestCase('\\', TokenType.Slash)]
    [TestCase('[', TokenType.LeftSquareBracket)]
    [TestCase(']', TokenType.RightSquareBracket)]
    [TestCase('(', TokenType.LeftParenthesis)]
    [TestCase(')', TokenType.RightParenthesis)]
    [TestCase('1', TokenType.Number)]
    public void GetTokenType_ShouldGetToken_Correctly(char input, TokenType expectedOutput)
    {
        var actualInput = MdLexer.GetTokenType(input);

        actualInput.Should().Be(expectedOutput);
    }
}