using Markdown.Tokens;

namespace MarkDownTests;

public class TokenizerTestsData
{
    public static IEnumerable<TestCaseData> TestData
    {
        get
        {
            yield return new TestCaseData("a b c", new List<Token>()
            {
                new LiteralToken(0, 4, "a b c")
            }).SetName("ShouldTokenizeInputToLiteralToken_WhenInputHasNoAnotherTokens");

            yield return new TestCaseData("a _b_", new List<Token>()
            {
                new LiteralToken(0, 1, "a "),
                new LiteralToken(3, 3, "b"),
                new ItalicsToken(2, 4) { IsCorrect = true },
            }).SetName("ShouldTokenizeInputToLiteralTokensAndItalicsToken_WhenInputContainsItalicsToken");

            yield return new TestCaseData("a __b__", new List<Token>()
            {
                new LiteralToken(0, 1, "a "),
                new LiteralToken(4, 4, "b"),
                new BoldToken(2, 6) { IsCorrect = true }
            }).SetName("ShouldTokenizeInputToLiteralTokensAndBoldToken_WhenInputContainsBoldToken");

            yield return new TestCaseData("# a b c", new List<Token>()
            {
                new ParagraphToken(0, 6) { IsCorrect = true },
                new LiteralToken(2, 6, "a b c")
            }).SetName("ShouldTokenizeInputToLiteralTokensAndItalicsToken_WhenInputContainsParagraphToken");

            yield return new TestCaseData("a \\ b c", new List<Token>()
            {
                new LiteralToken(0, 6, "a \\ b c")
            }).SetName("ShouldReturnScreeningSeparator_WhenItsNotScreening");

            yield return new TestCaseData("a \\_b_ c", new List<Token>()
            {
                new LiteralToken(0, 1, "a "),
                new LiteralToken(3, 7, "_b_ c")
            }).SetName("ShoudIgnoreSeparator_WhenItsScreened");

            yield return new TestCaseData("a_1_3 b__12", new List<Token>()
            {
                new LiteralToken(0, 10, "a_1_3 b__12")
            }).SetName("ShouldIgnoreSeparators_WhenSeparatorsInsideWordWithDigit");

            yield return new TestCaseData("a_a b_b", new List<Token>()
            {
                new LiteralToken(0, 0, "a"),
                new LiteralToken(2, 4, "a b"),
                new LiteralToken(6, 6, "b"),
                new LiteralToken(1, 1, "_"),
                new LiteralToken(5, 5, "_")
            }).SetName("ShouldReplaceTokenToLiteral_WhenSeparatorsInsideDiferentWords");

            yield return new TestCaseData("a ____ b", new List<Token>()
            {
                new LiteralToken(0, 1, "a "),
                new LiteralToken(2, 3, "__"),
                new LiteralToken(4, 5, "__"),
                new LiteralToken(6, 7, " b"),
            }).SetName("ShouldReplaceTokenToLiteral_WhenTokenHasNoContent");

            yield return new TestCaseData("__a _b__ c_", new List<Token>()
            {
                new LiteralToken(2, 3, "a "),
                new LiteralToken(5, 5, "b"),
                new LiteralToken(8, 9, "c"),
                new LiteralToken(0, 1, "__"),
                new LiteralToken(6, 7, "__"),
                new LiteralToken(4, 4, "_"),
                new LiteralToken(10, 10, "_")
            }).SetName("ShouldReplaceIntersectedTokens");

            yield return new TestCaseData("_ a_", new List<Token>()
            {
                new LiteralToken(0, 3, "_ a_"),
            }).SetName("ShouldIgnoreToken_WhenOpeningIndexIsNotValid");

            yield return new TestCaseData("_a _", new List<Token>()
            {
                new LiteralToken(1, 3, "a _"),
                new LiteralToken(0, 0, "_")
            }).SetName("ShouldIgnoreToken_WhenClosingIndexIsNotValid");

            yield return new TestCaseData("_abc \nabc_", new List<Token>()
            {
                new LiteralToken(0, 0, "_"),
                new LiteralToken(1, 5, "abc \n"),
                new LiteralToken(6, 9, "abc_")
            }).SetName("ShouldIgnoreToken_WhenSeparatorsInDifferenrParagraphs");

            yield return new TestCaseData("|*ПЕРВЫЙ ПУНКТ* *ВТОРОЙ ПУНКТ* *ТРЕТИЙ ПУНКТ*|", new List<Token>()
            {
                new LiteralToken(2, 13, "ПЕРВЫЙ ПУНКТ"),
                new ListItemToken(1, 14) { IsCorrect = true },
                new LiteralToken(15, 15, " ") { IsCorrect = true },
                new LiteralToken(17, 28, "ВТОРОЙ ПУНКТ"),
                new ListItemToken(16, 29) { IsCorrect = true },
                new LiteralToken(30, 30, " ") { IsCorrect = true },
                new LiteralToken(32, 43, "ТРЕТИЙ ПУНКТ"),
                new ListItemToken(31, 44) { IsCorrect = true },
                new MarkedListToken(0, 45) { IsCorrect = true },
            }).SetName("ShouldReturnMarkedListToken");

            yield return new TestCaseData("*ПЕРВЫЙ ПУНКТ*", new List<Token>()
            {
                new LiteralToken(1, 12, "ПЕРВЫЙ ПУНКТ"),
                new LiteralToken(0, 0, "*"),
                new LiteralToken(13, 13, "*")
            }).SetName("ShouldIgnoreListItemToken_WhenItNotCorrect");
        }
    }
}