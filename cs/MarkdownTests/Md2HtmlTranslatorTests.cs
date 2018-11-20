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
            var tokens = new Dictionary<TokenType, List<TokenPosition>>
            {
                {new TokenType("singleUnderscore", "_", "em", TokenLocationType.InlineToken), new List<TokenPosition> {new TokenPosition(0, 3)}}
            };

            var htmlText = translator.TranslateMdToHtml("_ff_", tokens);

            htmlText.ShouldBeEquivalentTo("<em>ff</em>");
        }

        [Test]
        public void GetHtmlText_AfterMdWithNestedSingleUnderscore()
        {
            var translator = new Md2HtmlTranslator();
            var tokens = new Dictionary<TokenType, List<TokenPosition>>
            {
                {new TokenType("singleUnderscore", "_", "em", TokenLocationType.InlineToken), new List<TokenPosition> {new TokenPosition(0, 8), new TokenPosition(3,5)}}
            };

            var htmlText = translator.TranslateMdToHtml("_f _f_ f_", tokens);

            htmlText.ShouldBeEquivalentTo("<em>f <em>f</em> f</em>");
        }

        [Test]
        public void GetHtmlText_AfterMdWithDoubleUnderscore()
        {
            var translator = new Md2HtmlTranslator();
            var tokens = new Dictionary<TokenType, List<TokenPosition>>
            {
                {new TokenType("doubleUnderscore", "__", "strong", TokenLocationType.InlineToken), new List<TokenPosition> {new TokenPosition(0, 4)}}
            };

            var htmlText = translator.TranslateMdToHtml("__ff__", tokens);

            htmlText.ShouldBeEquivalentTo("<strong>ff</strong>");
        }

        [Test]
        public void GetHtmlText_AfterMdWithSingleAndDoubleUnderscore()
        {
            var translator = new Md2HtmlTranslator();
            var tokens = new Dictionary<TokenType, List<TokenPosition>>
            {
                {new TokenType("doubleUnderscore", "__", "strong", TokenLocationType.InlineToken), new List<TokenPosition> {new TokenPosition(0, 4)}},
                {new TokenType("singleUnderscore", "_", "em", TokenLocationType.InlineToken), new List<TokenPosition> {new TokenPosition(7, 9)}}
            };

            var htmlText = translator.TranslateMdToHtml("__ff__ _f_", tokens);

            htmlText.ShouldBeEquivalentTo("<strong>ff</strong> <em>f</em>");
        }
    }
}
