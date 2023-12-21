using Markdown.Token;

namespace MarkdownTests;

public class AnySyntaxParserTestCases
{
    public static IEnumerable<TestCaseData> ParseTokenTestCases
    {
        get
        {
            yield return new TestCaseData(string.Empty, new List<IToken>())
                .SetName("ReturnEmptySequence_WhenStringIsEmpty");
            yield return new TestCaseData("a", new List<IToken>())
                .SetName("ReturnEmptySequence_WhenNoTokensInText");
            yield return new TestCaseData("____", new List<IToken>())
                .SetName("ReturnEmptySequence_WhenBoldTokenContentIsEmpty");
            yield return new TestCaseData("__", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenItalicTokenContentIsEmpty");
            yield return new TestCaseData("__a_a__a_", new List<IToken>())
                .SetName("ReturnEmptySequence_WhenTokensIntersect");
            yield return new TestCaseData("__a _a", new List<IToken>())
                .SetName("ReturnEmptySequence_WhenTwoUnpairedTokens");
            yield return new TestCaseData("__a_a\na_a__a", new List<IToken>())
                .SetName("ReturnEmptySequence_WhenTokenClosesAtNextLine");

            yield return new TestCaseData("__ a__", new List<IToken>())
                .SetName("ReturnEmptySequence_WhenBoldTokenOpenedIncorrectly");
            yield return new TestCaseData("__a __", new List<IToken>())
                .SetName("ReturnEmptySequence_WhenBoldTokenClosedIncorrectly");
            yield return new TestCaseData("a__a a__a", new List<IToken>())
                .SetName("ReturnEmptySequence_WhenBoldTokenInsideDifferentWords");
            yield return new TestCaseData("1__1__1", new List<IToken>())
                .SetName("ReturnEmptySequence_WhenBoldTokenInsideTextWithDigits");
            yield return new TestCaseData("\\a", new List<IToken>())
                .SetName("ReturnEmptySequence_WhenOrdinalSymbolEscaped");

            yield return new TestCaseData("_ a_", new List<IToken>())
                .SetName("ReturnEmptySequence_WhenThereIsIncorrectOpenItalicToken");
            yield return new TestCaseData("_a _", new List<IToken>())
                .SetName("ReturnEmptySequence_WhenThereIsIncorrectCloseItalicToken");
            yield return new TestCaseData("a_a a_a", new List<IToken>())
                .SetName("ReturnEmptySequence_WhenThereIsItalicTokenInDifferentWords");
            yield return new TestCaseData("1_1_1", new List<IToken>())
                .SetName("ReturnEmptySequence_WhenThereIsItalicTokenInTextWithDigits");

            yield return new TestCaseData("# a", new List<IToken>() { new HeaderToken(0), new HeaderToken(3, true) })
                .SetName("ReturnHeaderToken_WhenHeaderTokenProvided");
            yield return new TestCaseData("\\# a", new List<IToken>() { new EscapeToken(0) })
                .SetName("ReturnEscapingToken_WhenEscapedTokenProvided");
            yield return new TestCaseData("# a\n# a",
                    new List<IToken>() { new HeaderToken(0), new HeaderToken(3, true), new HeaderToken(4), new HeaderToken(7, true) })
                .SetName("ReturnTwoHeaderTokens_TwoHeaderTokensProvided");

            yield return new TestCaseData("_a_", new List<IToken>() { new ItalicToken(0), new ItalicToken(2) })
                .SetName("ReturnItalicToken_WhenItalicTokenProvided");
            yield return new TestCaseData("__a__", new List<IToken>() { new BoldToken(0), new BoldToken(3) })
                .SetName("ReturnBoldToken_WhenIsBoldTokenProvided");
            yield return new TestCaseData("_a__a__a_",
                    new List<IToken>() { new ItalicToken(0), new ItalicToken(8) })
                .SetName("ReturnItalicToken_WhenBoldTokenInsideItalicToken");
            yield return new TestCaseData("__a_a_a__",
                    new List<IToken>()
                        { new BoldToken(0), new ItalicToken(3), new ItalicToken(5), new BoldToken(7) })
                .SetName("ReturnBoldTokenAndItalicToken_WhenItalicTokenInsideBoldToken");

            yield return new TestCaseData("\\__a\\__", new List<IToken>()
                {
                    new EscapeToken(0), new EscapeToken(4)
                })
                .SetName("ReturnEscapingTokens_WhenBoldTokenIsEscaped");
            yield return new TestCaseData("\\_a\\_",
                    new List<IToken>() { new EscapeToken(0), new EscapeToken(3) })
                .SetName("ReturnEscapingTokens_WhenItalicTokenIsEscaped");
            yield return new TestCaseData("\\\\a", new List<IToken>() { new EscapeToken(0) })
                .SetName("ReturnEscapingToken_WhenEscapeTokenIsEscaped");
            
            yield return new TestCaseData("@aba caba@", new List<IToken>() { new ImageToken(0), new ImageToken(9, true) })
                .SetName("ReturnLinkToken_WhenLinkToken");
            yield return new TestCaseData("@#a__b__a c_a_ba@", new List<IToken>() { new ImageToken(0), new ImageToken(16, true) })
                .SetName("ReturnLinkToken_WhenLinkTokenContainsTokens");

            yield return new TestCaseData(
                    "# Text___with_different__tags\\__",
                    new List<IToken>()
                    {
                        new HeaderToken(0), new BoldToken(6),
                        new ItalicToken(8), new ItalicToken(13),
                        new BoldToken(23), new EscapeToken(29), new HeaderToken(32, true)
                    })
                .SetName("ReturnMultipleTokens_WhenStringContainsMultipleTokens");
        }
    }
}