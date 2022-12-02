using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownRenderShould
    {
        [Test]
        public void ReturnTextWhenNoTags()
        {
            var text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. abdc dsa Morbi nec.";
            var translator = new HtmlTranslator();
            var mdRender = new MarkdownRenderer(translator);

            var html = mdRender.Render(text);

            html.Should().Be(text);
        }


        [TestCase("__openBold word _openItalic word closeItalic_ word closeBold__",
            "<strong>openBold word <em>openItalic word closeItalic</em> word closeBold</strong>",
            TestName = "correct work with few words")]
        [TestCase("_a", "_a",
            TestName = "italic is not closed")]
        [TestCase("_a_", "<em>a</em>",
            TestName = "italic closed within the word")]
        [TestCase("__a__", "<strong>a</strong>",
            TestName = "bold closed within the word")]
        [TestCase("__BoldItalic_Italic_ItalicBold__",
            "<strong>BoldItalic<em>Italic</em>ItalicBold</strong>",
            TestName = "bold and italic closed within the word")]
        [TestCase("a_a a_a", "a_a a_a",
            TestName = "italics in different words in the middle")]
        [TestCase("__asd_das__", "__asd_das__",
            TestName = "not closed italics in words")]
        [TestCase("____", "____",
            TestName = "underscore without letters")]
        [TestCase("___a___", "<strong><em>a</em></strong>",
            TestName = "3 underlines in a row")]
        [TestCase("___a b___", "<strong><em>a b</em></strong>",
            TestName = "3 consecutive underscores with multiple words")]
        [TestCase(@"a__", "a__",
            TestName = "was not open bold")]
        [TestCase("_bold__", "_bold__",
            TestName = "not paired tags")]
        [TestCase("__intersection _bold__ and italic_", "__intersection _bold__ and italic_",
            TestName = "intersection of bold and italic")]
        [TestCase("_ab__cd_", "<em>ab__cd</em>",
            TestName = "bold inside italics within a word")]
        [TestCase("__a _b_ _c_ d__", "<strong>a <em>b</em> <em>c</em> d</strong>",
            TestName = "multiple closing tags inside the bold")]
        [TestCase("_a __c__ d_", "<em>a __c__ d</em>",
            TestName = "bold inside italics")]
        [TestCase("_ab_ad", "<em>ab</em>ad",
            TestName = "italics opens at the beginning closes in the middle")]
        [TestCase("a_b_ad", "a<em>b</em>ad",
            TestName = "italics opens in the middle closes in the middle")]
        [TestCase("a_bad_", "a<em>bad</em>",
            TestName = "italics opens in the middle closes at the end")]
        [TestCase("__bold a_bad_ bold__", "<strong>bold a<em>bad</em> bold</strong>",
            TestName = "italic opens in the middle closes at the end inside bold")]
        [TestCase("__bold a_b_ad bold__", "<strong>bold a<em>b</em>ad bold</strong>",
            TestName = "italic opens in the middle closes in the middle inside bold")]
        [TestCase("__bold _ab_ad bold__", "<strong>bold <em>ab</em>ad bold</strong>",
            TestName = "italic opens at the beginning closes in the middle inside bold")]
        public void CorrectlyWork_WithItalicAndBoldTags(string text, string expected)
        {
            var translator = new HtmlTranslator();
            var mdRender = new MarkdownRenderer(translator);

            var html = mdRender.Render(text);

            html.Should().Be(expected);
        }

        [TestCase(@"\_a_", "_a_", TestName = "shielded opening tag")]
        [TestCase(@"\\_a_", "\\<em>a</em>", TestName = "escapes slash")]
        [TestCase(@"_\a_", @"<em>\a</em>", TestName = "does not escape the letter")]
        [TestCase(@"_a\_", "_a_", TestName = "shielded closing tag")]
        [TestCase(@"\__a_", "_<em>a</em>", TestName = "shields the bold turning into italic")]
        [TestCase(@"_ab \#aba ba_", "<em>ab #aba ba</em>", TestName = "escapes special characters")]
        public void CorrectlyWork_WithShielding(string text, string expected)
        {
            var translator = new HtmlTranslator();
            var mdRender = new MarkdownRenderer(translator);

            var html = mdRender.Render(text);

            html.Should().Be(expected);
        }

        [TestCase("# aba", "<h1> aba</h1>",
            TestName = "correctly header")]
        [TestCase("#aba", "#aba",
            TestName = "sharp with word")]
        [TestCase("_ab #aba ba_", "<em>ab #aba ba</em>",
            TestName = "sharp in middle with word")]
        [TestCase("# __a _b_ _c_ d__",
            "<h1> <strong>a <em>b</em> <em>c</em> d</strong></h1>",
            TestName = "header works correctly with other tags")]
        [TestCase("ab #", "ab #",
            TestName = "sharp in end")]
        [TestCase("ab # ab", "ab # ab",
            TestName = "sharp lonely in middle")]
        [TestCase("# ", "<h1> </h1>",
            TestName = "correctly header with space")]
        [TestCase("#", "<h1></h1>",
            TestName = "works correctly when there is only sharp")]
        [TestCase("a #", "a #",
            TestName = "sharp lonely in end")]
        [TestCase("# abc\r\n# abc", "<h1> abc</h1>\r\n<h1> abc</h1>",
            TestName = "correct work with multiple paragraphs")]
        public void CorrectlyWork_WithHeader(string text, string expected)
        {
            var translator = new HtmlTranslator();
            var mdRender = new MarkdownRenderer(translator);

            var html = mdRender.Render(text);

            html.Should().Be(expected);
        }


        [TestCase("[1234](abc)", "<a href='abc' alt=''>1234</a>",
            TestName = "Correctly work link")]
        [TestCase("![1234](abc)", "<img href='abc' alt='1234'>",
            TestName = "Correctly work image")]
        [TestCase("![[1234]](abc)", "![[1234]](abc)",
            TestName = "wrong image")]
        [TestCase("[1234]](abc)", "[1234]](abc)",
            TestName = "wrong link")]
        [TestCase("# [1234](abc)", "<h1> <a href='abc' alt=''>1234</a></h1>",
            TestName = "work correctly with header")]
        [TestCase("# _ab [1234](abc) ca_",
            "<h1> <em>ab <a href='abc' alt=''>1234</a> ca</em></h1>",
            TestName = "work correctly with other tags")]
        public void CorrectlyWork_WithSingleTags(string text, string expected)
        {
            var translator = new HtmlTranslator();
            var mdRender = new MarkdownRenderer(translator);

            var html = mdRender.Render(text);

            html.Should().Be(expected);
        }
    }
}