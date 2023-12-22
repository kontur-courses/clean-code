using FluentAssertions;
using Markdown;
using MarkdownTest.TestData;

namespace MarkdownTest;

public class LexerTest
{
    [TestCaseSource(typeof(LexerTestData), nameof(LexerTestData.RightKindsData))]
    public void ParseRightKindsOrder(string expression, SyntaxKind[] kinds)
    {
        new Lexer(expression)
            .GetTokens()
            .Select(tok => tok.Kind)
            .Should()
            .BeEquivalentTo(kinds);
    }

    [TestCaseSource(typeof(LexerTestData), nameof(LexerTestData.RightTexts))]
    public void ParseWordsFromText(string expression, string[] words)
    {
        new Lexer(expression)
            .GetTokens()
            .Where(tok => tok.Kind == SyntaxKind.Text)
            .Select(tok => tok.Text)
            .Should()
            .BeEquivalentTo(words);
    }
}