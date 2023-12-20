using Markdown.Token;

namespace MarkdownTests;

public class AnySyntaxParserTestCases
{
    public static IEnumerable<TestCaseData> ParseTokenTestCases
    {
        get
        {
            yield return new TestCaseData(string.Empty, new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenStringIsEmpty");
            yield return new TestCaseData("a", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenThereAreNoMarkdowns");
            yield return new TestCaseData("____", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenBoldTokenContentIsEmpty");
            yield return new TestCaseData("__", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenItalicTokenContentIsEmpty");

            yield return new TestCaseData("_a__a_a__", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenThereIsStyleTokenIntersectionWhenItalicFirst");
            yield return new TestCaseData("__a_a__a_", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenThereIsStyleTokenIntersectionWhenBoldFirst");
            yield return new TestCaseData("__a _a", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenThereAreUnpairedTokens");
            yield return new TestCaseData("__a_a\na_a__a", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenThereAreUnpairedTokensInParagraph");

            yield return new TestCaseData("__ a__", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenThereIsIncorrectOpenBoldToken");
            yield return new TestCaseData("__a __", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenThereIsIncorrectCloseBoldToken");
            yield return new TestCaseData("a__a a__a", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenThereIsBoldTokenInDifferentWords");
            yield return new TestCaseData("1__1__1", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenThereIsBoldTokenInTextWithDigits");

            yield return new TestCaseData("_ a_", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenThereIsIncorrectOpenItalicToken");
            yield return new TestCaseData("_a _", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenThereIsIncorrectCloseItalicToken");
            yield return new TestCaseData("a_a a_a", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenThereIsItalicTokenInDifferentWords");
            yield return new TestCaseData("1_1_1", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenThereIsItalicTokenInTextWithDigits");

            yield return new TestCaseData("# a", new List<IToken>() { new HeaderToken(0) })
                .SetName("ReturnHeaderToken_WhenThereIsHeaderToken");
            yield return new TestCaseData("\\# a", new List<IToken>() { new EscapeToken(0) })
                .SetName("ReturnEscapingToken_WhenThereIsEscapedTag");
            yield return new TestCaseData("# a\n# a",
                    new List<IToken>() { new HeaderToken(0), new HeaderToken(4) })
                .SetName("ReturnHeaderToken_WhenThereAreManyHeaderTokensAndManyString");

            yield return new TestCaseData("_a_", new List<IToken>() { new ItalicToken(0), new ItalicToken(2, true) })
                .SetName("ReturnItalicToken_WhenThereIsItalicToken");
            yield return new TestCaseData("__a__", new List<IToken>() { new BoldToken(0), new BoldToken(3, true) })
                .SetName("ReturnBoldToken_WhenItsThereIsBoldToken");
            yield return new TestCaseData("_a__a__a_",
                    new List<IToken>() { new ItalicToken(0), new ItalicToken(8, true) })
                .SetName("ReturnItalicToken_WhenBoldTokenInsideItalicToken");
            yield return new TestCaseData("__a_a_a__",
                    new List<IToken>()
                        { new BoldToken(0), new ItalicToken(3), new ItalicToken(5, true), new BoldToken(7, true) })
                .SetName("ReturnBoldTokenAndItalicToken_WhenItalicTokenInsideBoldToken");

            yield return new TestCaseData("\\__a\\__", new List<IToken>()
                {
                    new EscapeToken(0), new EscapeToken(4)
                })
                .SetName("ReturnEscapingTokens_WhenBoldTokenIsEscaped");
            yield return new TestCaseData("\\_a\\_",
                    new List<IToken>() { new EscapeToken(0), new EscapeToken(3) })
                .SetName("ReturnEscapingTokens_WhenThereIsScreenedItalicToken");
            yield return new TestCaseData("\\\\a", new List<IToken>() { new EscapeToken(0) })
                .SetName("ReturnEscapingToken_WhenThereIsScreenedEscapingToken");
            yield return new TestCaseData("\\a", new List<IToken>())
                .SetName("ReturnEmptyIEnumerable_WhenThereIsNothingToEscape");

            yield return new TestCaseData(
                    "#Text___with_different__tags\\__",
                    new List<IToken>()
                    {
                        new HeaderToken(0), new BoldToken(5),
                        new ItalicToken(7), new ItalicToken(12),
                        new BoldToken(22)
                    })
                .SetName("ReturnAllTokens_WhenThereIsMultipleTokens");
        }
    }
}