using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using System.Linq;

namespace Markdown
{
    internal class MdTests
    {
        [Test]
        public void Md_ShoudRenderHeadingStyle_InTextWithOneParagraph()
        {
            Md.Render("#heading").Should().BeEquivalentTo("<h1>heading</h1>");
        }

        [Test]
        public void Md_ShoudRenderHeadingStyle_InTextWithMoreThanOneParagraph()
        {
            Md.Render("#heading1\nnotheading\n#heading2").Should().
                BeEquivalentTo("<h1>heading1</h1>\nnotheading\n<h1>heading2</h1>");
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
        public void Md_ShoudRenderItalicStyle_InHeadingStyle()
        {
            Md.Render("#_italic_").Should().BeEquivalentTo("<h1><em>italic</em></h1>");
        }

        [Test]
        public void Md_ShoudRenderBoldStyle_InHeadingStyle()
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

        //[Test]
        //public void Md_ShoudNotRenderBoldStyle_InItalictyle()
        //{
        //    Md.Render("_ita__bold__ld_").Should().BeEquivalentTo("<em>ita__bold__ld</em>");
        //}
    }
}
