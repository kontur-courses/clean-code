using FluentAssertions;
using Markdown.Tokenizer;

namespace Markdown.Tests.TokenizerTests;

public static class TokenAssert
{
    public static void AssertToken(string markdownText, List<Token> expectedTokens)
    {
        var tokenizer = new MarkdownTokenizer(markdownText);

        var tokens = tokenizer.Tokenize();

        tokens.Should().BeEquivalentTo(expectedTokens);
    }
}