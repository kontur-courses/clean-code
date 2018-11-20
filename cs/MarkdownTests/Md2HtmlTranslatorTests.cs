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
            var simpleUnderScore = new TokenType("singleUnderscore", "_", "em", TokenLocationType.InlineToken);
            var translator = new Md2HtmlTranslator();
            var tokens = new List<SingleToken>
            {
                new SingleToken(simpleUnderScore, 0, LocationType.Opening),
                new SingleToken(simpleUnderScore, 3, LocationType.Closing)
            };

            var htmlText = translator.TranslateMdToHtml("_ff_", tokens);

            htmlText.ShouldBeEquivalentTo("<em>ff</em>");
        }

        [Test]
        public void GetHtmlText_AfterMdWithNestedSingleUnderscore()
        {
            var simpleUnderScore = new TokenType("singleUnderscore", "_", "em", TokenLocationType.InlineToken);
            var translator = new Md2HtmlTranslator();
            var tokens = new List<SingleToken>
            {
                new SingleToken(simpleUnderScore, 0, LocationType.Opening),
                new SingleToken(simpleUnderScore, 3, LocationType.Opening),
                new SingleToken(simpleUnderScore, 5, LocationType.Closing),
                new SingleToken(simpleUnderScore, 8, LocationType.Closing)
            };

            var htmlText = translator.TranslateMdToHtml("_f _f_ f_", tokens);

            htmlText.ShouldBeEquivalentTo("<em>f <em>f</em> f</em>");
        }

        [Test]
        public void GetHtmlText_AfterMdWithDoubleUnderscore()
        {
            var doubleUnderScore = new TokenType("doubleUnderscore", "__", "strong", TokenLocationType.InlineToken);
            var translator = new Md2HtmlTranslator();
            var tokens = new List<SingleToken>
            {
                new SingleToken(doubleUnderScore, 0, LocationType.Opening),
                new SingleToken(doubleUnderScore, 4, LocationType.Closing)
            };

            var htmlText = translator.TranslateMdToHtml("__ff__", tokens);

            htmlText.ShouldBeEquivalentTo("<strong>ff</strong>");
        }

        [Test]
        public void GetHtmlText_AfterMdWithSingleAndDoubleUnderscore()
        {
            var doubleUnderScore = new TokenType("doubleUnderscore", "__", "strong", TokenLocationType.InlineToken);
            var simpleUnderScore = new TokenType("singleUnderscore", "_", "em", TokenLocationType.InlineToken);

            var translator = new Md2HtmlTranslator();
            var tokens = new List<SingleToken>
            {
                new SingleToken(doubleUnderScore, 0, LocationType.Opening),
                new SingleToken(doubleUnderScore, 4, LocationType.Closing),
                new SingleToken(simpleUnderScore, 7, LocationType.Opening),
                new SingleToken(simpleUnderScore, 9, LocationType.Closing),
            };

            var htmlText = translator.TranslateMdToHtml("__ff__ _f_", tokens);

            htmlText.ShouldBeEquivalentTo("<strong>ff</strong> <em>f</em>");
        }
    }
}
