using Markdown;
using Markdown.Tokens.StringToken;

namespace MarkdownTest.TestData;

public class ParserTestData
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
                new[] { StringTokenType.SingleUnderscore }
            )
            .SetName("OneSingleUnderScore"),
        new TestCaseData(
                "_ _",
                new[] { StringTokenType.SingleUnderscore, StringTokenType.WhiteSpace, StringTokenType.SingleUnderscore }
            )
            .SetName("TwoSingleUnderScore_DividedByWhitespace"),
        new TestCaseData(
                "_  _",
                new[] { StringTokenType.SingleUnderscore, StringTokenType.WhiteSpace, StringTokenType.SingleUnderscore }
            )
            .SetName("TwoSingleUnderScore_DividedByWhitespaces"),
        new TestCaseData(
                "_abc_",
                new[] { StringTokenType.SingleUnderscore, StringTokenType.Text, StringTokenType.SingleUnderscore }
            )
            .SetName("TwoSingleUnderScoreDividedByWord"),
        new TestCaseData(
                "_a_",
                new[] { StringTokenType.SingleUnderscore, StringTokenType.Text, StringTokenType.SingleUnderscore }
            )
            .SetName("TwoSingleUnderScoreDividedBySingleLetterWord"),
        new TestCaseData(
                "__",
                new[] { StringTokenType.DoubleUnderscore }
            )
            .SetName("OneDoubleUnderscore"),
        new TestCaseData(
                "____",
                new[] { StringTokenType.DoubleUnderscore, StringTokenType.DoubleUnderscore }
            )
            .SetName("TwoDoubleUnderscore"),
        new TestCaseData(
                "__a__",
                new[] { StringTokenType.DoubleUnderscore, StringTokenType.Text, StringTokenType.DoubleUnderscore }
            )
            .SetName("TwoDoubleUnderScore_DividedBySingleLetterWord"),
        new TestCaseData(
                "__ __",
                new[] { StringTokenType.DoubleUnderscore, StringTokenType.WhiteSpace, StringTokenType.DoubleUnderscore }
            )
            .SetName("TwoDoubleUnderScore_DividedByWhitespace"),
        new TestCaseData(
                "__  __",
                new[] { StringTokenType.DoubleUnderscore, StringTokenType.WhiteSpace, StringTokenType.DoubleUnderscore }
            )
            .SetName("TwoDoubleUnderScore_DividedByWhitespaces"),
        new TestCaseData(
                "__abc__",
                new[] { StringTokenType.DoubleUnderscore, StringTokenType.Text, StringTokenType.DoubleUnderscore }
            )
            .SetName("TwoDoubleUnderScore_DividedByWord"),
        new TestCaseData(
                "___",
                new[] { StringTokenType.DoubleUnderscore, StringTokenType.SingleUnderscore }
            )
            .SetName("TwoDoubleUnderScoreAndUnderscoreInRow"),
        new TestCaseData(
                "# abc\n",
                new[] { StringTokenType.Hash, StringTokenType.WhiteSpace, StringTokenType.Text, StringTokenType.NewLine }
            )
            .SetName("HashWhitespaceWordAndNewLine"),
        
    };
}