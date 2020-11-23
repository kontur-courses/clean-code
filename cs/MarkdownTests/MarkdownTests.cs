using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownTests
    {
        [TestCase("_a_", ExpectedResult = "<em>a</em>", TestName = "WhenOneItalicField")]
        [TestCase("__a__", ExpectedResult = "<strong>a</strong>", TestName = "WhenOneBoldField")]
        [TestCase("\\_a\\_", ExpectedResult = "_a_", TestName = "WhenEscapedMarkdowns")]
        [TestCase("\\a\\", ExpectedResult = "\\a\\", TestName = "WhenEscapingSymbolsEscapeNothing")]
        [TestCase(@"\\_a_", ExpectedResult = @"\<em>a</em>", TestName = "Render_WhenEscapingSymbolIsEscaped")]
        [TestCase("__a _b_ c__", ExpectedResult = "<strong>a <em>b</em> c</strong>",
            TestName = "WhenItalicIsInsideBold")]
        [TestCase("_a __b__ c_", ExpectedResult = "<em>a __b__ c</em>", TestName = "WhenBoldIsInsideItalic")]
        [TestCase("_e_1_", ExpectedResult = "<em>e_1</em>", TestName = "WhenMarkdownIsInsideTextWithDigits")]
        [TestCase("_a_b a_b_c a_b_", ExpectedResult = "<em>a</em>b a<em>b</em>c a<em>b</em>",
            TestName = "WhenMarkdownsAreInsideWord")]
        [TestCase("a_b a_b", ExpectedResult = "a_b a_b", TestName = "WhenMarkdownsAreInsideDifferentWord")]
        [TestCase("__a_ b", ExpectedResult = "__a_ b", TestName = "WhenUnpairedMarkdownsInOneParagraph")]
        [TestCase("_ a_", ExpectedResult = "_ a_", TestName = "WhenWhitespaceAfterOpeningMarkdown")]
        [TestCase("_a _", ExpectedResult = "_a _", TestName = "WhenWhitespaceBeforeClosingMarkdown")]
        [TestCase("__a _b__ c_", ExpectedResult = "__a _b__ c_", TestName = "WhenItalicAndBoldAreIntersected")]
        [TestCase("____", ExpectedResult = "____", TestName = "WhenEmptyLineInMarkdowns")]
        [TestCase("# a", ExpectedResult = "<h1> a</h1>", TestName = "WhenTitle")]
        [TestCase("# a __b _c_ d__", ExpectedResult = "<h1> a <strong>b <em>c</em> d</strong></h1>",
            TestName = "WhenTitleWithAnotherMarkdowns")]
        [TestCase("a # a", ExpectedResult = "a # a", TestName = "WhenTitleMarkdownNotAtStartOfParagraph")]
        public string Render(string markdownText)
        {
            return Markdown.Markdown.Render(markdownText);
        }
    }
}