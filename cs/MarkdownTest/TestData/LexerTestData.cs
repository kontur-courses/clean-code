using Markdown;

namespace MarkdownTest.TestData;

public class LexerTestData
{
    public static TestCaseData[] RightTexts =
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

    public static TestCaseData[] RightKindsData =
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
        new TestCaseData(
                "# abc\n",
                new[] { SyntaxKind.Hash, SyntaxKind.Whitespace, SyntaxKind.Text, SyntaxKind.NewLine }
            )
            .SetName("HashWhitespaceWordAndNewLine"),
        new TestCaseData(
                "(abc)",
                new[] { SyntaxKind.OpenRoundBracket, SyntaxKind.Text, SyntaxKind.CloseRoundBracket }
            )
            .SetName("WordBetweenRoundBrackets"),
        new TestCaseData(
                "[abc]",
                new[] { SyntaxKind.OpenSquareBracket, SyntaxKind.Text, SyntaxKind.CloseSquareBracket }
            )
            .SetName("WordBetweenSquareBrackets")
    };
}