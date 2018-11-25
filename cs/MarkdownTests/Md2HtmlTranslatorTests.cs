using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Markdown;

namespace MarkdownTests
{
    [TestFixture]
    class Md2HtmlTranslatorTests
    {
        private readonly TokenType singleUnderscore = new TokenType(TokenTypeEnum.SingleUnderscore, "_", "em", TokenLocationType.InlineToken);
        private readonly TokenType doubleUnderscore = new TokenType(TokenTypeEnum.DoubleUnderscore, "__", "strong", TokenLocationType.InlineToken);

        [Test]
        public void GetHtmlText_AfterMdWithSingleUnderscore()
        {
            var translator = new Md2HtmlTranslator();
            var tokens = new List<SingleToken>
            {
                new SingleToken(singleUnderscore, 0, LocationType.Opening),
                new SingleToken(singleUnderscore, 3, LocationType.Closing),
            };

            var paragraphs = new TokenValidator().GetParagraphsWithValidTokens(tokens, "_ff_");

            var htmlText = translator.TranslateMdToHtml("_ff_", paragraphs);

            htmlText.ShouldBeEquivalentTo("<p><em>ff</em></p>");
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

            var paragraphs = new TokenValidator().GetParagraphsWithValidTokens(tokens, "_f _f_ f_");

            var htmlText = translator.TranslateMdToHtml("_f _f_ f_", paragraphs);

            htmlText.ShouldBeEquivalentTo("<p><em>f <em>f</em> f</em></p>");
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

            var paragraphs = new TokenValidator().GetParagraphsWithValidTokens(tokens, "__ff__");

            var htmlText = translator.TranslateMdToHtml("__ff__", paragraphs);

            htmlText.ShouldBeEquivalentTo("<p><strong>ff</strong></p>");
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
            var paragraphs = new TokenValidator().GetParagraphsWithValidTokens(tokens, "__ff__ _f_");

            var htmlText = translator.TranslateMdToHtml("__ff__ _f_", paragraphs);

            htmlText.ShouldBeEquivalentTo("<p><strong>ff</strong> <em>f</em></p>");
        }
    }
}
