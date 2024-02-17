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
                new ItalicsToken(2, 4),
            }).SetName("ShouldTokenizeInputToLiteralTokensAndItalicsToken_WhenInputContainsItalicsToken");

            yield return new TestCaseData("a __b__", new List<Token>()
            {
                new LiteralToken(0, 1, "a "),
                new LiteralToken(4, 4, "b"),
                new BoldToken(2, 6)
            }).SetName("ShouldTokenizeInputToLiteralTokensAndBoldToken_WhenInputContainsBoldToken");

            yield return new TestCaseData("#a b c", new List<Token>()
            {
                new ParagraphToken(0, 5),
                new LiteralToken(1, 5, "a b c")
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

            yield return new TestCaseData("a_1_3 b_12", new List<Token>()
            {
                new LiteralToken(0, 9, "a_1_3 b_12")
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
        }
    }
}