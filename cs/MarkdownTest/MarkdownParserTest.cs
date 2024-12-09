using FluentAssertions;
using Markdown;
using Markdown.Tokens.StringToken;
using MarkdownTest.TestData;

namespace MarkdownTest;

public class LexerTest
{
    [TestCaseSource(typeof(ParserTestData), nameof(ParserTestData.RightKindsData))]
    public void ParseRightKindsOrder(string expression, StringTokenType[] kinds)
    {
        var result = new MarkdownParser()
            .Parse(expression)
            .Select(tok => tok.Type).ToList();
        result.Should()
            .BeEquivalentTo(kinds);
    }

    [TestCaseSource(typeof(ParserTestData), nameof(ParserTestData.RightTexts))]
    public void ParseWordsFromText(string expression, string[] words)
    {
        new MarkdownParser()
            .Parse(expression)
            .Where(tok => tok.Type == StringTokenType.Text)
            .Select(tok => tok.Value)
            .Should()
            .BeEquivalentTo(words);
    }
}