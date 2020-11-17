using System.Linq;
using FluentAssertions;
using Markdown.Core;
using NUnit.Framework;

namespace MarkdownTests
{
    public class Tokenizer_Should
    {
        [Test]
        public void ParseIntoTokens_EmptyString_ReturnsEmptyCollection() =>
            Tokenizer.ParseIntoTokens("").Should().BeEmpty();

        [Test]
        public void ParseIntoTokens_StringWithoutMdTags_ReturnsStringToken() =>
            GetActualTokenAsHtml("some").Should().Be("some");

        [TestCase("__some")]
        [TestCase("some__")]
        [TestCase("_some__")]
        [TestCase("__some_")]
        public void ParseIntoTokens_UnclosedBoldTag_ReturnsStringToken(string input) =>
            GetActualTokenAsHtml(input).Should().Be(input);

        [Test]
        public void ParseIntoTokens_PlainMdItalicText_ReturnsCorrectItalicToken() =>
            GetActualTokenAsHtml("_some_").Should().Be("<em>some</em>");

        [Test]
        public void ParseIntoTokens_PlainMdHeaderText_ReturnsCorrectHeaderToken() =>
            GetActualTokenAsHtml("# some")
                .Should()
                .Be("<h1>some</h1>");

        [Test]
        public void ParseIntoTokens_HeaderTagNotAtBegin_ReturnsStringToken() =>
            GetActualTokenAsHtml("abc# some")
                .Should()
                .Be("abc# some");

        [Test]
        public void ParseIntoTokens_ComplexMdBoldText_ReturnsCorrectBoldToken() =>
            GetActualTokenAsHtml("__some _meaningful_ text__")
                .Should()
                .Be("<strong>some <em>meaningful</em> text</strong>");

        [Test]
        public void ParseIntoTokens_ComplexMdTextWithAllTags_ReturnsCorrectTokens() =>
            GetActualTokenAsHtml("# __some _meaningful_ text__")
                .Should()
                .Be("<h1><strong>some <em>meaningful</em> text</strong></h1>");

        [TestCase(@"_ text_")]
        [TestCase(@"__ text__")]
        [TestCase(@"_text _")]
        [TestCase(@"__text __")]
        [TestCase(@"__ text __")]
        public void ParseIntoTokens_WhiteSpaceAroundTag_ReturnsStringToken(string rawToken) =>
            GetActualTokenAsHtml(rawToken).Should().Be(rawToken);

        [TestCase(@"_text_1")]
        [TestCase(@"1_text_")]
        [TestCase(@"tex_1t_")]
        [TestCase(@"tex_t1_")]
        public void ParseIntoTokens_DigitAroundItalicToken_ReturnStringToken(string rawToken) =>
            GetActualTokenAsHtml(rawToken).Should().Be(rawToken);

        [TestCase(@"\_text\_")]
        [TestCase(@"\_\_text\_\_")]
        [TestCase(@"\[text](url.com)")]
        public void ParseIntoTokens_EscapedTag_ReturnsStringToken(string rawToken) =>
            GetActualTokenAsHtml(rawToken)
                .Should()
                .Be(rawToken.Replace("\\", ""));

        [Test]
        public void ParseIntoTokens_EscapedEscapeSymbol_ReturnsEscapeSymbolAndCorrectToken() =>
            Tokenizer.ParseIntoTokens(@"\\_text_")
                .ConvertToHtmlString()
                .Should()
                .Be(@"\<em>text</em>");

        [Test]
        public void ParseIntoTokens_BoldTokenInsideItalicToken_ReturnsCorrectItalicToken() =>
            GetActualTokenAsHtml(@"_some __beautiful__ text_").Should().Be("<em>some __beautiful__ text</em>");

        [Test]
        public void ParseIntoTokens_LinkTokenInsideItalicToken_ReturnsItalicWithLinkTokenInside() =>
            GetActualTokenAsHtml("_by [this](url.com) article_")
                .Should()
                .Be("<em>by <a href=\"url.com\">this</a> article</em>");

        [Test]
        public void ParseIntoTokens_LinkTokenInsideBoldToken_ReturnsBoldTokenWithLinkInside() =>
            GetActualTokenAsHtml("__by [this](url.com) article__")
                .Should()
                .Be("<strong>by <a href=\"url.com\">this</a> article</strong>");

        private static string GetActualTokenAsHtml(string mdTag) =>
            Tokenizer.ParseIntoTokens(mdTag).First().ToHtmlString();
    }
}