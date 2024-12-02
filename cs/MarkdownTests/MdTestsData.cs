namespace MarkdownTests;

public class MdTestsData
{
    public static IEnumerable<TestCaseData> TestData
    {
        get
        {
            yield return new TestCaseData("a b c", "a b c").SetName("ShouldReturnSameString_WhenThereAreNoTokens");

            yield return new TestCaseData("b _окруженный с двух сторон_", "b <em>окруженный с двух сторон</em>").SetName(
                "ShouldReturnWithItalicsToken_WhenItalicsInInput");

            yield return new TestCaseData("b __окруженный с двух сторон__", "b <strong>окруженный с двух сторон</strong>").SetName(
                "ShouldReturnWithBoldToken_WhenBoldInInput");
            
            yield return new TestCaseData("Здесь сим\\волы экранирования\\ \\должны остаться.\\", 
                "Здесь сим\\волы экранирования\\ \\должны остаться.\\").SetName(
                "ShouldReturnWithScreeningSeparator_WhenItDoesNotScreen");

            yield return new TestCaseData("a \\\\ b c", "a \\ b c").SetName(
                "ShouldReturnWithScreeningToken_WhenItScreen");
            
            yield return new TestCaseData("a \\_b_ c", "a _b_ c").SetName(
                "ShouldNotReturnItalicsToken_WhenItsScreened");

            yield return new TestCaseData("a \\__b__ c", "a __b__ c").SetName(
                "ShouldNotReturnBoldToken_WhenItScreened");

            yield return new TestCaseData("_ a b_", "_ a b_").SetName(
                "ShouldIgnoreInvalidOpeningItalicsSeparator");
            
            yield return new TestCaseData("__ a b__", "__ a b__").SetName(
                "ShouldIgnoreInvalidOpeningBoldSeparator");

            yield return new TestCaseData("a _a b _c", "a _a b _c").SetName(
                "ShouldIgnoreInvalidClosingItalicsSeparator");
            
            yield return new TestCaseData("a __a b __c", "a __a b __c").SetName(
                "ShouldIgnoreInvalidClosingBoldsSeparator");

            yield return new TestCaseData("цифрами_12_3", "цифрами_12_3").SetName(
                "ShouldNotReturnTokens_WhenSeparatorsIsInsideWordWithDigit");

            yield return new TestCaseData("_нач_але", "<em>нач</em>але").SetName(
                "ShouldReturnItalicsToken_WhenItHighlightStartOfWord");
            
            yield return new TestCaseData("сер_еди_не", "сер<em>еди</em>не").SetName(
                "ShouldReturnItalicsToken_WhenItHighlightCenterOfWord");

            yield return new TestCaseData("кон_це._", "кон<em>це.</em>").SetName(
                "ShouldReturnItalicsToken_WhenItHighlightEndOfWord");
            
            yield return new TestCaseData("__нач__але", "<strong>нач</strong>але").SetName(
                "ShouldReturnBoldToken_WhenItHighlightStartOfWord");
            
            yield return new TestCaseData("сер__еди__не", "сер<strong>еди</strong>не").SetName(
                "ShouldReturnBoldToken_WhenItHighlightCenterOfWord");

            yield return new TestCaseData("кон__це.__", "кон<strong>це.</strong>").SetName(
                "ShouldReturnBoldToken_WhenItHighlightEndOfWord");

            yield return new TestCaseData("ра_зных сл_овах", "ра_зных сл_овах").SetName(
                "ShouldNotReturnToken_WhenSeparatorsIsInsideDifferentWords");
            
            yield return new TestCaseData("____", "____").SetName(
                "ShouldNotReturnToken_WhenTokenHasNoContent");

            yield return new TestCaseData("__a _b_ c__", "<strong>a <em>b</em> c</strong>").SetName(
                "ShouldReturnItalicsToken_WhenItIsInsideBoldToken");

            yield return new TestCaseData("_a __b__ c_", "<em>a __b__ c</em>").SetName(
                "ShouldNotReturnBoldToken_WhenItIsInsideItalicsToken");

            yield return new TestCaseData("__пересечения _двойных__ и одинарных_", 
                "__пересечения _двойных__ и одинарных_").SetName(
                "ShouldNotReturnTokens_WhenTokensIsIntersecting");

            yield return new TestCaseData("# a ", "<h1>a </h1>").SetName(
                "ShouldReturnParagraphToken_WhenParagraphTokenIsInInput");

            yield return new TestCaseData("# a # b c", "<h1>a # b c</h1>").SetName(
                "ShouldIgnoreParagraphSeparator_WhenItIsNotInStartOfParagraph");

            yield return new TestCaseData("# a b c \n d e", "<h1>a b c </h1>\n d e").SetName(
                "ShouldCloseParagraphToken_WhenParagraphIsEnding");
           
            yield return new TestCaseData("# Заголовок __с _разными_ символами__", 
                    "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")
                .SetName("ShouldReturnParagraphWithDifferentTokens");
        }
    }
}