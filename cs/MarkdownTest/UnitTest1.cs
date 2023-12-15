using FluentAssertions;
using Markdown;

namespace MarkdownTest;

public class LexerTest
{
    public static TestCaseData[] rightKindsData =
    {
        new TestCaseData(
                "_",
                new[] { SyntaxKind.SingleUnderscore }
            )
            .SetName("OneSingleUnderScore"),
        new TestCaseData(
                "_ _",
                new[] { SyntaxKind.SingleUnderscore, SyntaxKind.Whitespace, SyntaxKind.SingleUnderscore }
            )
            .SetName("TwoSingleUnderScore_DividedByWhitespace"),
        new TestCaseData(
                "_  _",
                new[] { SyntaxKind.SingleUnderscore, SyntaxKind.Whitespace, SyntaxKind.SingleUnderscore }
            )
            .SetName("TwoSingleUnderScore_DividedByWhitespaces"),
        new TestCaseData(
                "_abc_",
                new[] { SyntaxKind.SingleUnderscore, SyntaxKind.Text, SyntaxKind.SingleUnderscore }
            )
            .SetName("TwoSingleUnderScoreDividedByWord"),
        new TestCaseData(
                "_a_",
                new[] { SyntaxKind.SingleUnderscore, SyntaxKind.Text, SyntaxKind.SingleUnderscore }
            )
            .SetName("TwoSingleUnderScoreDividedBySingleLetterWord"),
        new TestCaseData(
                "__",
                new[] { SyntaxKind.DoubleUnderscore }
            )
            .SetName("OneDoubleUnderscore"),
        new TestCaseData(
                "____",
                new[] { SyntaxKind.DoubleUnderscore, SyntaxKind.DoubleUnderscore }
            )
            .SetName("TwoDoubleUnderscore"),
        new TestCaseData(
                "__a__",
                new[] { SyntaxKind.DoubleUnderscore, SyntaxKind.Text, SyntaxKind.DoubleUnderscore }
            )
            .SetName("TwoDoubleUnderScore_DividedBySingleLetterWord"),
        new TestCaseData(
                "__ __",
                new[] { SyntaxKind.DoubleUnderscore, SyntaxKind.Whitespace, SyntaxKind.DoubleUnderscore }
            )
            .SetName("TwoDoubleUnderScore_DividedByWhitespace"),
        new TestCaseData(
                "__  __",
                new[] { SyntaxKind.DoubleUnderscore, SyntaxKind.Whitespace, SyntaxKind.DoubleUnderscore }
            )
            .SetName("TwoDoubleUnderScore_DividedByWhitespaces"),
        new TestCaseData(
                "__abc__",
                new[] { SyntaxKind.DoubleUnderscore, SyntaxKind.Text, SyntaxKind.DoubleUnderscore }
            )
            .SetName("TwoDoubleUnderScore_DividedByWord"),
        new TestCaseData(
                "___",
                new[] { SyntaxKind.DoubleUnderscore, SyntaxKind.SingleUnderscore }
            )
            .SetName("TwoDoubleUnderScoreAndUnderscoreInRow"),
    };

    public static TestCaseData[] rightTexts =
    {
        new TestCaseData("a", new[] { "a" })
            .SetName("SingleLetterWords"),
        new TestCaseData("abc", new[] { "abc" })
            .SetName("SingleWord"),
        new TestCaseData("ab_cd", new[] { "ab", "cd" })
            .SetName("TwoWords_SeparatedBySingleUnderscore"),
        new TestCaseData("ab__cd", new[] { "ab", "cd" })
            .SetName("TwoWords_SeparatedByDoubleUnderscore"),
        new TestCaseData("ab cd", new[] { "ab", "cd" })
            .SetName("TwoWords_SeparatedByWhitespace"),
        new TestCaseData("abc__def_ghi jkl", new[] { "abc", "def", "ghi", "jkl" })
            .SetName("ThreeWords_SeparatedByDifferentTags"),
        new TestCaseData(@"\_", new[] { "_" })
            .SetName("ShieldedUnderscore"),
        new TestCaseData(@"\__", new[] { "_" })
            .SetName("ShieldedUnderscoreAndSingleUnderscore"),
        new TestCaseData(@"\___", new[] { "_" })
            .SetName("ShieldedUnderscoreAndDoubleUnderscore"),
        new TestCaseData(@"_\__", new[] { "_" })
            .SetName("ShieldedUnderscoreBetweenSingleUnderscores"),
        new TestCaseData(@"\_\_", new[] { "__" })
            .SetName("TwoShieldedUnderscoresInRow"),
    };
    
    [TestCaseSource(nameof(rightKindsData))]
    public void ParseRightKindsOrder(string expression, SyntaxKind[] kinds)
    {
        var lexer = new Lexer(expression);
        lexer.GetTokens()
            .Select(tok => tok.Kind)
            .Should()
            .BeEquivalentTo(kinds);
    }

    [TestCaseSource(nameof(rightTexts))]
    public void ParseWordsFromText(string expression, string[] words)
    {
        var lexer = new Lexer(expression);
        lexer.GetTokens()
            .Where(tok => tok.Kind == SyntaxKind.Text)
            .Select(tok => tok.Text)
            .Should()
            .BeEquivalentTo(words);
    }
}