using Markdown.Filter;
using Markdown.Filter.MarkdownFilters;
using Markdown.Lexer;
using MarkdownTests.Lexer.TestCases;

namespace MarkdownTests.Lexer;

public class MarkdownLexerBuilderTests
{
    [TestCaseSource(typeof(LexerBuilderTestCases), nameof(LexerBuilderTestCases.InvalidParametersTests))]
    public void WithTokenType_ThrowsArgumentException_OnInvalidParameters(LexerBuilderTestData builderTestData)
    {
        Assert.Throws<ArgumentException>(()
            => new MarkdownLexerBuilder(new MarkdownFilter()).WithTokenType(builderTestData.TokenType));
    }
}