using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class MdTests
    {
        [Test]
        public void Md_ShouldRenderHeadingStyle_InTextWithOneParagraph()
        {
            Md.Render("#heading").Should().BeEquivalentTo("<h1>heading</h1>");
        }

        [Test]
        public void Md_ShouldRenderHeadingStyle_InTextWithMoreThanOneParagraph()
        {
            Md.Render("#heading1\r\nnotheading\r\n#heading2").Should()
                .BeEquivalentTo("<h1>heading1</h1>\r\nnotheading\r\n<h1>heading2</h1>");
        }

        [Test]
        public void Md_ShouldRenderBoldStyle_WithOneWordInContent()
        {
            Md.Render("__bold__").Should().BeEquivalentTo("<strong>bold</strong>");
        }

        [Test]
        public void Md_ShouldRenderItalicStyle_WithOneWordInContent()
        {
            Md.Render("_italic_").Should().BeEquivalentTo("<em>italic</em>");
        }

        [Test]
        public void Md_ShouldRenderItalicStyle_InHeadingStyle()
        {
            Md.Render("#_italic_").Should().BeEquivalentTo("<h1><em>italic</em></h1>");
        }

        [Test]
        public void Md_ShouldRenderBoldStyle_InHeadingStyle()
        {
            Md.Render("#__bold__").Should().BeEquivalentTo("<h1><strong>bold</strong></h1>");
        }

        [Test]
        public void Md_ShoudRenderItalicStyle_InBoldStyle()
        {
            Md.Render("__bo_italic_ld__").Should().BeEquivalentTo("<strong>bo<em>italic</em>ld</strong>");
        }

        [Test]
        public void Md_ShouldRenderItalicStyle_InBoldStyle_InHeadingStyle()
        {
            Md.Render("#__bo_italic_ld__").Should().BeEquivalentTo("<h1><strong>bo<em>italic</em>ld</strong></h1>");
        }

        [TestCase("b__old__", "b<strong>old</strong>")]
        [TestCase("b__ol__d", "b<strong>ol</strong>d")]
        [TestCase("__bol__d", "<strong>bol</strong>d")]
        public void Md_ShouldRenderBoldStyle_InsideWordWithoutDigits(string mdtext, string expectedHtmlText)
        {
            Md.Render(mdtext).Should().BeEquivalentTo(expectedHtmlText);
        }

        [TestCase("b__old1__", "b__old1__")]
        [TestCase("b__ol__d1", "b__ol__d1")]
        [TestCase("__bol__d1", "__bol__d1")]
        [TestCase("__bol__d1__", "<strong>bol__d1</strong>")]
        [TestCase("te1xt __w1th__ nu1mbers", "te1xt <strong>w1th</strong> nu1mbers")]
        [TestCase("__te1xt with nu1mbers__", "<strong>te1xt with nu1mbers</strong>")]
        [TestCase("__12__ 3", "<strong>12</strong> 3")]
        public void Md_ShouldNotRenderBoldStyle_InsideWordWithDigits(string mdtext, string expectedHtmlText)
        {
            Md.Render(mdtext).Should().BeEquivalentTo(expectedHtmlText);
        }

        [TestCase("i_talic_", "i<em>talic</em>")]
        [TestCase("i_tali_c", "i<em>tali</em>c")]
        [TestCase("_itali_c", "<em>itali</em>c")]
        public void Md_ShouldRenderItalicStyle_InsideWordWithoutDigits(string mdtext, string expectedHtmlText)
        {
            Md.Render(mdtext).Should().BeEquivalentTo(expectedHtmlText);
        }

        [TestCase("i_talic1_", "i_talic1_")]
        [TestCase("i_tali_c1", "i_tali_c1")]
        [TestCase("_itali_c1", "_itali_c1")]
        [TestCase("_itali_c1_", "<em>itali_c1</em>")]
        [TestCase("te1xt _w1th_ nu1mbers", "te1xt <em>w1th</em> nu1mbers")]
        [TestCase("_te1xt with nu1mbers_", "<em>te1xt with nu1mbers</em>")]
        [TestCase("_12_ 3", "<em>12</em> 3")]
        public void Md_ShouldNotRenderItalicStyle_InsideWordWithDigits(string mdtext, string expectedHtmlText)
        {
            Md.Render(mdtext).Should().BeEquivalentTo(expectedHtmlText);
        }

        [Test]
        public void Md_ShouldRenderItalicStyle_WithMoreThanOneWordsInContent()
        {
            Md.Render("_italic italic_").Should().BeEquivalentTo("<em>italic italic</em>");
        }

        [Test]
        public void Md_ShouldRenderBoldStyle_WithMoreThanOneWordsInContent()
        {
            Md.Render("__bold bold__").Should().BeEquivalentTo("<strong>bold bold</strong>");
        }

        [Test]
        public void Md_ShouldNotRenderItalicStyle_WithStartAndEndInsideDifferentWords()
        {
            Md.Render("i_talic itali_c").Should().BeEquivalentTo("i_talic itali_c");
        }

        [Test]
        public void Md_ShouldNotRenderBoldStyle_WithStartAndEndInsideDifferentWords()
        {
            Md.Render("b_old bol_d").Should().BeEquivalentTo("b_old bol_d");
        }

        [Test]
        public void Md_ShouldNotRenderBoldStyle_WithEmptyContent()
        {
            Md.Render("____").Should().BeEquivalentTo("____");
        }

        [Test]
        public void Md_ShouldNotRenderItalicStyle_WithEmptyContent()
        {
            Md.Render("__").Should().BeEquivalentTo("__");
        }

        [TestCase("_ italic_", "_ italic_")]
        [TestCase("_\titalic_", "_\titalic_")]
        [TestCase("_\nitalic_", "_\nitalic_")]
        public void Md_ShouldNotRenderItalicStyle_WithWhiteSpaceAfterStartTag(string mdText, string expectedHTMLText)
        {
            Md.Render(mdText).Should().BeEquivalentTo(expectedHTMLText);
        }

        [TestCase("_ bold_", "_ bold_")]
        [TestCase("_\tbold_", "_\tbold_")]
        [TestCase("_\nbold_", "_\nbold_")]
        public void Md_ShouldNotRenderBoldStyle_WithWhiteSpaceAfterStartTag(string mdText, string expectedHTMLText)
        {
            Md.Render(mdText).Should().BeEquivalentTo(expectedHTMLText);
        }

        [Test]
        public void Md_ShouldNotRenderBoldStyle_InItalicStyle()
        {
            Md.Render("_ita__bold__ld_").Should().BeEquivalentTo("<em>ita__bold__ld</em>");
        }

        [Test]
        public void Md_ShouldNotRenderStyle_WithUnpairedTags()
        {
            Md.Render("__unpaired_").Should().BeEquivalentTo("__unpaired_");
        }

        [Test]
        public void Md_ShouldNotRenderBoldAndItalicStyle_WithIntersection()
        {
            Md.Render("__inter_sect__ion_").Should().BeEquivalentTo("__inter_sect__ion_");
        }

        [TestCase("____bold____", "<strong><strong>bold</strong></strong>")]
        [TestCase("______bold______", "<strong><strong><strong>bold</strong></strong></strong>")]
        [TestCase("____bo ld____", "<strong><strong>bo ld</strong></strong>")]
        [TestCase("______bo ld______", "<strong><strong><strong>bo ld</strong></strong></strong>")]
        public void Md_ShouldRenderOnlyBoldStyle_WhenUnderscoreCountIsEven(string mdText, string expectedHTMLText)
        {
            Md.Render(mdText).Should().BeEquivalentTo(expectedHTMLText);
        }

        [TestCase("___bold___", "___bold___")]
        [TestCase("___bo ld___", "___bo ld___")]
        [TestCase("_____bold_____", "_____bold_____")]
        [TestCase("_____bo ld_____", "_____bo ld_____")]
        public void Md_ShouldNotRenderBoldAndItalicStyle_WhenUnderscoreCountIsOdd(string mdText,
            string expectedHTMLText)
        {
            Md.Render(mdText).Should().BeEquivalentTo(expectedHTMLText);
        }

        [TestCase("\\_word_", "_word_")]
        [TestCase("_word\\_", "_word_")]
        [TestCase("\\_word\\_", "_word_")]
        [TestCase("\\__word__", "__word__")]
        [TestCase("__word\\__", "__word__")]
        [TestCase("\\__word\\__", "_<em>word_</em>")]
        [TestCase("\\#word", "#word")]
        [TestCase("\\___word__", "_<strong>word</strong>")]
        [TestCase("wo\\_rd_", "wo_rd_")]
        [TestCase("\\+ul", "+ul")]
        public void MdRender_ShouldIgnoreEscapedChars(string mdText, string expectedHTMLText)
        {
            Md.Render(mdText).Should().BeEquivalentTo(expectedHTMLText);
        }

        [TestCase("\\\\_word_", "\\<em>word</em>")]
        [TestCase("\\\\\\_word_", "\\_word_")]
        [TestCase("\\\\word", "\\\\word")]
        [TestCase("\\\\+ul", "\\+ul")]
        public void MdRender_ShouldDeleteEscapingChars(string mdText, string expectedHTMLText)
        {
            Md.Render(mdText).Should().BeEquivalentTo(expectedHTMLText);
        }

        [Test]
        public void Md_ShouldRenderUnorderedList_WithOneElement()
        {
            Md.Render("+ ul").Should().BeEquivalentTo("<ul><li>ul</li></ul>");
        }

        [Test]
        public void Md_ShouldRenderUnorderedList_WithEmptyElement()
        {
            Md.Render("+ ").Should().BeEquivalentTo("<ul><li></li></ul>");
        }

        [Test]
        public void Md_ShouldRenderUnorderedList_WithMoreThanOneElement()
        {
            Md.Render("+ ul\r\n+ ul").Should().BeEquivalentTo("<ul><li>ul</li>\r\n<li>ul</li></ul>");
        }

        [Test]
        public void Md_ShouldRenderUnorderedList_WithNesting()
        {
            Md.Render("+ ul\r\n  + ul\r\n    + ul").Should().BeEquivalentTo(
                "<ul><li>ul<ul>\r\n<li>ul<ul>\r\n<li>ul</li></ul></li></ul></li></ul>");
        }

        [Test]
        public void Md_ShouldRenderItalicStyle_InUnorderedList()
        {
            Md.Render("+ _ul_").Should().BeEquivalentTo("<ul><li><em>ul</em></li></ul>");
        }

        [Test]
        public void Md_ShouldRenderBoldStyle_InUnorderedList()
        {
            Md.Render("+ __ul__").Should().BeEquivalentTo("<ul><li><strong>ul</strong></li></ul>");
        }
    }
}