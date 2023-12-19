using Markdown.Filter.MarkdownFilters;
using Markdown.Lexer;
using Markdown.Tokens.Types;
using MarkdownTests.Lexer.TestCases;

namespace MarkdownTests.Lexer;

public class MarkdownLexerBuilderTests
{
    [TestCaseSource(typeof(LexerBuilderTestCases), nameof(LexerBuilderTestCases.InvalidParametersTests))]
    public void WithTokenType_ThrowsArgumentException_OnInvalidParameters(ITokenType tokenType)
    {
        Assert.Throws<ArgumentException>(()
            => new MarkdownLexerBuilder(new MarkdownFilter()).WithTokenType(tokenType));
    }
}