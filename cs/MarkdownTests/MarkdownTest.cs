using System.Collections;
using FluentAssertions;
using Markdown;

namespace MarkdownTests;

[TestFixture]
public class MarkdownTest
{
    [Test]
    public void MarkdownRender_ReturnTextWhenNoTags()
    {
        var text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis feugiat placerat sagittis.";
        
        var generatedText = Md.Render(text);

        generatedText.Should().Be(text);
    }

    [TestCaseSource(nameof(ItalicAndBoldTestCases))]
    public void MarkdownRender_CorrectlyWork_WithItalicAndBoldTags(string text, string expected)
    {
        var generatedText = Md.Render(text);

        generatedText.Should().Be(expected);
    }

    public static IEnumerable ItalicAndBoldTestCases
    {
        get
        {
            yield return new TestCaseData("__openBold word _openItalic word closeItalic word_ closeBold__",
                "<strong>openBold word <em>openItalic word closeItalic word</em> closeBold</strong>")
                .SetName("Italic tags in Bold are allowed");
            yield return new TestCaseData("_a", "_a").SetName("italic is not closed");
            yield return new TestCaseData("_a_", "<em>a</em>").SetName("italic closed within the word");
            yield return new TestCaseData("__a__", "<strong>a</strong>").SetName("italic closed within the word");
            yield return new TestCaseData("__BoldItalic_Italic_ItalicBold__",
                "<strong>BoldItalic<em>Italic</em>ItalicBold</strong>")
                .SetName("bold and italic closed within the word");
            yield return new TestCaseData("a_a a_a", "a_a a_a").SetName("italics in different words in the middle");
            yield return new TestCaseData("__asd_das__", "<strong>asd_das</strong>").SetName("not closed italics in words");
            yield return new TestCaseData("____", "____").SetName("underscore without letters");
            yield return new TestCaseData("_bold__", "_bold__").SetName("not paired tags");
            yield return new TestCaseData("__intersection _bold__ and italic_", "__intersection _bold__ and italic_")
                .SetName("intersection of bold and italic");
            yield return new TestCaseData("_ab__cd_", "<em>ab__cd</em>").SetName("bold inside italics within a word");
            yield return new TestCaseData("__a _b_ _c_ d__", "<strong>a <em>b</em> <em>c</em> d</strong>")
                .SetName("multiple closing tags inside the bold");
            yield return new TestCaseData("_a __c__ d_", "<em>a __c__ d</em>").SetName("bold inside italics");
            yield return new TestCaseData("_ab_ad", "<em>ab</em>ad").SetName("italics opens at the beginning closes in the middle");
            yield return new TestCaseData("a_bad_", "a<em>bad</em>").SetName("italics opens in the middle closes at the end");
            yield return new TestCaseData("__bold _ab_ad bold__", "<strong>bold <em>ab</em>ad bold</strong>")
                .SetName("italic opens at the beginning closes in the middle inside bold");
            yield return new TestCaseData("_a123_", "_a123_").SetName("italic not works with digit");
            yield return new TestCaseData("__a123__", "__a123__").SetName("bold not works with digit");
            yield return new TestCaseData("__test _ _markdown_ text__ another text", "<strong>test _ <em>markdown</em> text</strong> another text")
                .SetName("underscore and space is not opening tag");
        }
    }

    [TestCaseSource(nameof(ShieldingTestCases))]
    public void MarkdownRender_CorrectlyWork_WithShielding(string text, string expected)
    {
        var generatedText = Md.Render(text);

        generatedText.Should().Be(expected);
    }

    public static IEnumerable ShieldingTestCases
    {
        get
        {
            yield return new TestCaseData("\\_a_", "_a_").SetName("shielded opening tag");
            yield return new TestCaseData("\\\\_a_", "\\<em>a</em>").SetName("ignore shield backslash");
            yield return new TestCaseData("_\\a_", "<em>\\a</em>").SetName("does not escape the letter");
            yield return new TestCaseData("_a\\_", "_a_").SetName("shielded closing tag");
            yield return new TestCaseData("\\__a_", "_<em>a</em>").SetName("shields the bold turning into italic");
            yield return new TestCaseData("__test \\_ _markdown_ text__ another text", "<strong>test _ <em>markdown</em> text</strong> another text")
                .SetName("shielding does not interfere with the work of other tags");
        }
    }

    [TestCaseSource(nameof(HeaderTestCases))]
    public void CorrectlyWork_WithHeader(string text, string expected)
    {
        var generatedText = Md.Render(text);

        generatedText.Should().Be(expected);
    }

    public static IEnumerable HeaderTestCases
    {
        get
        {
            yield return new TestCaseData("# aba", "<h1>aba</h1>").SetName("correctly header");
            yield return new TestCaseData("#aba", "#aba").SetName("sharp with word");
            yield return new TestCaseData("# __a _b_ _c_ d__",
                "<h1><strong>a <em>b</em> <em>c</em> d</strong></h1>")
                .SetName("header works correctly with other tags");
            yield return new TestCaseData("ab #", "ab #").SetName("sharp in end");
            yield return new TestCaseData("ab # ab", "ab # ab").SetName("sharp in middle of string");
            yield return new TestCaseData("# abc\n# abc", "<h1>abc</h1>\n<h1>abc</h1>")
                .SetName("correct work with multiple paragraphs");
        }
    }
}