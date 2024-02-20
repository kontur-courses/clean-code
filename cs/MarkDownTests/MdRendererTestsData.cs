namespace MarkDownTests;

public class MdRendererTestsData
{
    public static IEnumerable<TestCaseData> TestData
    {
        get
        {
            yield return new TestCaseData("a b c", "a b c").SetName("ShouldReturnTheSameString_WhenThereAreNoTokens");

            yield return new TestCaseData("a _b_", "a <em>b</em>").SetName(
                "ShouldPrintItalicToken_When_ItalicTokenInInput");

            yield return new TestCaseData("a __b__", "a <strong>b</strong>").SetName(
                "ShouldPrintBoldToken_When_BoldTokenInInput");
            
            yield return new TestCaseData("_ a b_", "_ a b_").SetName(
                "ShouldIgnoreInvalidOpeningSeparator");
            
            yield return new TestCaseData("a _a b _c_ ab", "a <em>a b _c</em> ab").SetName(
                "ShouldIgnoreInvalidClosingSeparator");
            
            yield return new TestCaseData("a_1_3 b__12", "a_1_3 b__12").SetName(
                "ShouldNotPrintTokens_WhenSeparatorsInsideWordWithDigit");

            yield return new TestCaseData("_ab_c", "<em>ab</em>c").SetName(
                "ShouldPrintToken_WhenItHighlightStartOfWord");
            
            yield return new TestCaseData("a_bc_", "a<em>bc</em>").SetName(
                "ShouldPrintToken_WhenItHighlightEndOfWord");
            
            yield return new TestCaseData("a_b_c", "a<em>b</em>c").SetName(
                "ShouldPrintToken_WhenItHighlightCenterOfWord");

            yield return new TestCaseData("a_a b_b", "a_a b_b").SetName(
                "ShouldNotPrintToken_WhenSeparatorsInsideDiferentWords");

            yield return new TestCaseData("a ____ b", "a ____ b").SetName(
                "ShouldNotPrintToken_WhenTokenHasNoContent");
            
            yield return new TestCaseData("__a _b_ c__", "<strong>a <em>b</em> c</strong>").SetName(
                "ShouldNotPrintToken_WhenItInsideTokenWhichCanContainAnotherTags");

            yield return new TestCaseData("_a __b__ c_", "<em>a __b__ c</em>").SetName(
                "ShouldNotPrintToken_WhenItInsideTokenWhichCantContainAnotherTags");
            
            yield return new TestCaseData("__a _b__ c_", "__a _b__ c_").SetName(
                "ShouldNotPrintTokens_WhenTokensIsIntersecting");
            
            yield return new TestCaseData("# a ", "<h1>a </h1>").SetName(
                "ShouldPrintParagraphToken_When_ParagraphTokenInInput");
            
            yield return new TestCaseData("# a# b c", "<h1>a# b c</h1>").SetName(
                "ShouldIgnoreParagraphSeparator_WhenItNotInStartOfParagraph");

            yield return new TestCaseData("# ", "# ").SetName(
                "ShouldIgnoreParagraphToken_WhenTokenIsEmpty");
            
            yield return new TestCaseData("# a b c \n d e", "<h1>a b c </h1>\n d e").SetName(
                "ShouldCloseParagraphToken_WhenParagraphIsEnding");

            yield return new TestCaseData("a \\ b c", "a \\ b c").SetName(
                "ShouldPrintScreeningSeparator_WhenItsNotScreening");

            yield return new TestCaseData("a \\\\ b c", "a \\ b c").SetName(
                "ShouldPrintScreeningToken_WhenItIsScreening");

            yield return new TestCaseData("a \\_b_ c", "a _b_ c").SetName(
                "ShoudNotPrintToken_WhenItsScreened");

            yield return new TestCaseData("a \\__b_ c", "a _<em>b</em> c").SetName(
                "ShouldIgnoreSeparatorSymbol_WhenItScreened");
            
            yield return new TestCaseData("# Заголовок __с _разными_ символами__",
                    "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")
                .SetName("ShouldPrintParagraphWithDifferentTokens");

            yield return new TestCaseData("|*ПЕРВЫЙ ПУНКТ* *ВТОРОЙ ПУНКТ* *ТРЕТИЙ ПУНКТ*|",
                    "<ul><li>ПЕРВЫЙ ПУНКТ</li> <li>ВТОРОЙ ПУНКТ</li> <li>ТРЕТИЙ ПУНКТ</li></ul>")
                .SetName("ShouldPrintMarkedListToken");
            
        }
    }
}