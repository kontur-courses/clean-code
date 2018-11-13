using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Markdown;

namespace MarkdownTests
{
    [TestFixture]
    class Md2HtmlTranslatorTests
    {
        [Test]
        public void GetHtmlText_AfterMdWithSingleUnderscore()
        {
            var translator = new Md2HtmlTranslator();
            var markups = new Dictionary<Markup, List<MarkupPosition>>
            {
                {new Markup("singleUnderscore", "_", "em"), new List<MarkupPosition> {new MarkupPosition(0, 3)}}
            };

            var htmlText = translator.TranslateMdToHtml("_ff_", markups);

            htmlText.ShouldBeEquivalentTo("<em>ff</em>");
        }

        [Test]
        public void GetHtmlText_AfterMdWithNestedSingleUnderscore()
        {
            var translator = new Md2HtmlTranslator();
            var markups = new Dictionary<Markup, List<MarkupPosition>>
            {
                {new Markup("singleUnderscore", "_", "em"), new List<MarkupPosition> {new MarkupPosition(0, 8), new MarkupPosition(3,5)}}
            };

            var htmlText = translator.TranslateMdToHtml("_f _f_ f_", markups);

            htmlText.ShouldBeEquivalentTo("<em>f <em>f</em> f</em>");
        }

        [Test]
        public void GetHtmlText_AfterMdWithDoubleUnderscore()
        {
            var translator = new Md2HtmlTranslator();
            var markups = new Dictionary<Markup, List<MarkupPosition>>
            {
                {new Markup("doubleUnderscore", "__", "strong"), new List<MarkupPosition> {new MarkupPosition(0, 4)}}
            };

            var htmlText = translator.TranslateMdToHtml("__ff__", markups);

            htmlText.ShouldBeEquivalentTo("<strong>ff</strong>");
        }

        [Test]
        public void GetHtmlText_AfterMdWithSingleAndDoubleUnderscore()
        {
            var translator = new Md2HtmlTranslator();
            var markups = new Dictionary<Markup, List<MarkupPosition>>
            {
                {new Markup("doubleUnderscore", "__", "strong"), new List<MarkupPosition> {new MarkupPosition(0, 4)}},
                {new Markup("singleUnderscore", "_", "em"), new List<MarkupPosition> {new MarkupPosition(7, 9)}}
            };

            var htmlText = translator.TranslateMdToHtml("__ff__ _f_", markups);

            htmlText.ShouldBeEquivalentTo("<strong>ff</strong> <em>f</em>");
        }
    }
}
