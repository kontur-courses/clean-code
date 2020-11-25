using FluentAssertions;
using Markdown.Core;
using NUnit.Framework;

namespace MarkdownTests
{
    public class HtmlConverter_Should
    {
        [Test]
        public void ConvertToHtml_EmptyString_ReturnsEmptyCollection() =>
            HtmlConverter.ConvertToHtmlString("").Should().BeEmpty();

        [Test]
        public void ConvertToHtml_StringWithoutMdTags_ReturnsStringToken() =>
            GetResultAsHtml("some").Should().Be("some");

        [TestCase("__some")]
        [TestCase("some__")]
        [TestCase("_some__")]
        [TestCase("__some_")]
        public void ConvertToHtml_UnclosedBoldTag_ReturnsStringToken(string input) =>
            GetResultAsHtml(input).Should().Be(input);

        [Test]
        public void ConvertToHtml_PlainMdItalicText_ReturnsCorrectItalicToken() =>
            GetResultAsHtml("_some_").Should().Be("<em>some</em>");

        [Test]
        public void ConvertToHtml_PlainMdHeaderText_ReturnsCorrectHeaderToken() =>
            GetResultAsHtml("# some")
                .Should()
                .Be("<h1>some</h1>");

        [Test]
        public void ConvertToHtml_HeaderTagNotAtBegin_ReturnsStringToken() =>
            GetResultAsHtml("abc# some")
                .Should()
                .Be("abc# some");

        [Test]
        public void ConvertToHtml_ComplexMdBoldText_ReturnsCorrectBoldToken() =>
            GetResultAsHtml("__some _meaningful_ text__")
                .Should()
                .Be("<strong>some <em>meaningful</em> text</strong>");

        [Test]
        public void ConvertToHtml_ComplexMdTextWithAllTags_ReturnsCorrectTokens() =>
            GetResultAsHtml("# __some _meaningful_ text__")
                .Should()
                .Be("<h1><strong>some <em>meaningful</em> text</strong></h1>");

        [TestCase(@"_ text_")]
        [TestCase(@"__ text__")]
        [TestCase(@"_text _")]
        [TestCase(@"__text __")]
        [TestCase(@"__ text __")]
        public void ConvertToHtml_WhiteSpaceAroundTag_ReturnsStringToken(string rawToken) =>
            GetResultAsHtml(rawToken).Should().Be(rawToken);

        [TestCase(@"_text_1")]
        [TestCase(@"1_text_")]
        [TestCase(@"tex_1t_")]
        [TestCase(@"tex_t1_")]
        public void ConvertToHtml_DigitAroundItalicToken_ReturnStringToken(string rawToken) =>
            GetResultAsHtml(rawToken).Should().Be(rawToken);

        [TestCase(@"\_text\_")]
        [TestCase(@"\_\_text\_\_")]
        [TestCase(@"\[text](url.com)")]
        public void ConvertToHtml_EscapedTag_ReturnsStringToken(string rawToken) =>
            GetResultAsHtml(rawToken)
                .Should()
                .Be(rawToken.Replace("\\", ""));

        [Test]
        public void ConvertToHtml_EscapedEscapeSymbol_ReturnsEscapeSymbolAndCorrectToken() =>
            HtmlConverter.ConvertToHtmlString(@"\\_text_")
                .Should()
                .Be(@"\<em>text</em>");

        [Test]
        public void ConvertToHtml_BoldTokenInsideItalicToken_ReturnsCorrectItalicToken() =>
            GetResultAsHtml(@"_some __beautiful__ text_").Should().Be("<em>some __beautiful__ text</em>");

        [Test]
        public void ConvertToHtml_LinkTokenInsideItalicToken_ReturnsItalicWithLinkTokenInside() =>
            GetResultAsHtml("_by [this](url.com) article_")
                .Should()
                .Be("<em>by <a href=\"url.com\">this</a> article</em>");

        [Test]
        public void ConvertToHtml_LinkTokenInsideBoldToken_ReturnsBoldTokenWithLinkInside() =>
            GetResultAsHtml("__by [this](url.com) article__")
                .Should()
                .Be("<strong>by <a href=\"url.com\">this</a> article</strong>");

        [Test]
        public void ConvertToHtml_SpaceBeforeClosingTag_ReturnsStringToken() =>
            GetResultAsHtml("_foo _bar").Should().Be("_foo _bar");

        private static string GetResultAsHtml(string mdTag) =>
            HtmlConverter.ConvertToHtmlString(mdTag);
    }
}