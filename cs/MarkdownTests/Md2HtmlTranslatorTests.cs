using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Markdown;

namespace MarkdownTests
{
    [TestFixture]
    class Md2HtmlTranslatorTests
    {
        private readonly TokenType singleUnderscore = new TokenType("singleUnderscore", "_", "em", TokenLocationType.InlineToken);
        private readonly TokenType doubleUnderscore = new TokenType("doubleUnderscore", "__", "strong", TokenLocationType.InlineToken);

        [Test]
        public void GetHtmlText_AfterMdWithSingleUnderscore()
        {
            var translator = new Md2HtmlTranslator();
            var tokens = new List<SingleToken>
            {
                new SingleToken(singleUnderscore, 0, LocationType.Opening),
                new SingleToken(singleUnderscore, 3, LocationType.Closing)
            };

            var htmlText = translator.TranslateMdToHtml("_ff_", tokens);

            htmlText.ShouldBeEquivalentTo("<em>ff</em>");
        }

        [Test]
        public void GetHtmlText_AfterMdWithNestedSingleUnderscore()
        {
            var translator = new Md2HtmlTranslator();
            var tokens = new List<SingleToken>
            {
                new SingleToken(singleUnderscore, 0, LocationType.Opening),
                new SingleToken(singleUnderscore, 3, LocationType.Opening),
                new SingleToken(singleUnderscore, 5, LocationType.Closing),
                new SingleToken(singleUnderscore, 8, LocationType.Closing)
            };

            var htmlText = translator.TranslateMdToHtml("_f _f_ f_", tokens);

            htmlText.ShouldBeEquivalentTo("<em>f <em>f</em> f</em>");
        }

        [Test]
        public void GetHtmlText_AfterMdWithDoubleUnderscore()
        {
            var translator = new Md2HtmlTranslator();
            var tokens = new List<SingleToken>
            {
                new SingleToken(doubleUnderscore, 0, LocationType.Opening),
                new SingleToken(doubleUnderscore, 4, LocationType.Closing)
            };

            var htmlText = translator.TranslateMdToHtml("__ff__", tokens);

            htmlText.ShouldBeEquivalentTo("<strong>ff</strong>");
        }

        [Test]
        public void GetHtmlText_AfterMdWithSingleAndDoubleUnderscore()
        {
            var translator = new Md2HtmlTranslator();
            var tokens = new List<SingleToken>
            {
                new SingleToken(doubleUnderscore, 0, LocationType.Opening),
                new SingleToken(doubleUnderscore, 4, LocationType.Closing),
                new SingleToken(singleUnderscore, 7, LocationType.Opening),
                new SingleToken(singleUnderscore, 9, LocationType.Closing),
            };

            var htmlText = translator.TranslateMdToHtml("__ff__ _f_", tokens);

            htmlText.ShouldBeEquivalentTo("<strong>ff</strong> <em>f</em>");
        }
    }
}
