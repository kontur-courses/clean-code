using System.Linq;
using FluentAssertions;
using Markdown.Core;
using Markdown.TokenModels;
using NUnit.Framework;

namespace MarkdownTests
{
    public class Tokenizer_Should
    {
        [Test]
        public void ParseIntoTokens_EmptyString_ReturnsEmptyCollection() =>
            Tokenizer.ParseIntoTokens("").Should().BeEmpty();

        [Test]
        public void ParseIntoTokens_StringWithoutMdTags_ReturnsStringToken()
        {
            var actual = Tokenizer.ParseIntoTokens("some").First().ToHtmlString();
            actual.Should().Be(StringToken.Create("some").ToHtmlString());
        }

        [Test]
        public void ParseIntoTokens_UnclosedItalicTag_ReturnsStringToken()
        {
            var actual = Tokenizer.ParseIntoTokens("_some").First().ToHtmlString();
            actual.Should().Be(StringToken.Create("_some").ToHtmlString());
        }

        [TestCase("__some")]
        [TestCase("some__")]
        [TestCase("_some__")]
        [TestCase("__some_")]
        public void ParseIntoTokens_UnclosedBoldTag_ReturnsStringToken(string input)
        {
            var actual = Tokenizer.ParseIntoTokens(input).First().ToHtmlString();
            actual.Should().Be(StringToken.Create(input).ToHtmlString());
        }

        [Test]
        public void ParseIntoTokens_PlainMdItalicText_ReturnsCorrectItalicToken()
        {
            const string mdText = "_some_";

            var expectedToken = ItalicToken.Create("_some_", 0).ToHtmlString();
            var actualToken = Tokenizer.ParseIntoTokens(mdText).First().ToHtmlString();

            actualToken.Should().Be(expectedToken);
        }

        [Test]
        public void ParseIntoTokens_PlainMdHeaderText_ReturnsCorrectHeaderToken()
        {
            const string mdText = "# some";

            const string expectedToken = "<h1>some</h1>";
            var actualToken = Tokenizer.ParseIntoTokens(mdText).First().ToHtmlString();

            actualToken.Should().Be(expectedToken);
        }
        
        [Test]
        public void ParseIntoTokens_HeaderTagNotAtBegin_ReturnsStringToken()
        {
            const string mdText = "abc# some";

            const string expectedToken = "abc# some";
            var actualToken = Tokenizer.ParseIntoTokens(mdText).First().ToHtmlString();

            actualToken.Should().Be(expectedToken);
        }

        [Test]
        public void ParseIntoTokens_ComplexMdBoldText_ReturnsCorrectBoldToken()
        {
            const string mdText = "__some _meaningful_ text__";
            const string expectedTokenAsHtml = "<strong>some <em>meaningful</em> text</strong>";

            var actualToken = Tokenizer.ParseIntoTokens(mdText).First();
            actualToken.ToHtmlString().Should().Be(expectedTokenAsHtml);
        }

        [Test]
        public void ParseIntoTokens_ComplexMdTextWithAllTags_ReturnsCorrectTokens()
        {
            const string mdText = "# __some _meaningful_ text__";
            const string expectedTokenAsHtml = "<h1><strong>some <em>meaningful</em> text</strong></h1>";

            var actualToken = Tokenizer.ParseIntoTokens(mdText).First();
            actualToken.ToHtmlString().Should().Be(expectedTokenAsHtml);
        }

        [TestCase(@"_ text_")]
        [TestCase(@"__ text__")]
        [TestCase(@"_text _")]
        [TestCase(@"__text __")]
        [TestCase(@"__ text __")]
        public void ParseIntoTokens_WhiteSpaceAroundTag_ReturnsStringToken(string rawToken)
        {
            var actualToken = Tokenizer.ParseIntoTokens(rawToken).First();
            actualToken.ToHtmlString().Should().Be(StringToken.Create(rawToken).ToHtmlString());
        }

        [TestCase(@"_text_1")]
        [TestCase(@"1_text_")]
        [TestCase(@"tex_1t_")]
        [TestCase(@"tex_t1_")]
        public void ParseIntoTokens_DigitAroundItalicToken_ReturnStringToken(string rawToken)
        {
            var actualToken = Tokenizer.ParseIntoTokens(rawToken).First();
            actualToken.ToHtmlString().Should().Be(StringToken.Create(rawToken).ToHtmlString());
        }

        [TestCase(@"\_text\_")]
        [TestCase(@"\_\_text\_\_")]
        [TestCase(@"\[text](url.com)")]
        public void ParseIntoTokens_EscapedTag_ReturnsStringToken(string rawToken)
        {
            var actualToken = Tokenizer.ParseIntoTokens(rawToken).First();
            actualToken.ToHtmlString().Should().Be(StringToken.Create(rawToken.Replace("\\", "")).ToHtmlString());
        }

        [Test]
        public void ParseIntoTokens_EscapedEscapeSymbol_ReturnsEscapeSymbolAndCorrectToken()
        {
            const string mdTag = @"\\_text_";
            var actualToken = Tokenizer.ParseIntoTokens(mdTag).ConvertToHtmlString();
            actualToken.Should().Be(@"\<em>text</em>");
        }

        [Test]
        public void ParseIntoTokens_BoldTokenInsideItalicToken_ReturnsCorrectItalicToken()
        {
            const string mdTag = @"_some __beautiful__ text_";
            var actualToken = Tokenizer.ParseIntoTokens(mdTag).First().ToHtmlString();
            actualToken.Should().Be("<em>some __beautiful__ text</em>");
        }

        [Test]
        public void ParseIntoTokens_LinkTokenInsideItalicToken_ReturnsItalicWithLinkTokenInside()
        {
            var actualToken = Tokenizer.ParseIntoTokens("_by [this](url.com) article_").First().ToHtmlString();
            actualToken.Should().Be("<em>by <a href=\"url.com\">this</a> article</em>");
        }

        [Test]
        public void ParseIntoTokens_LinkTokenInsideBoldToken_ReturnsBoldTokenWithLinkInside()
        {
            var actualToken = Tokenizer.ParseIntoTokens("__by [this](url.com) article__").First().ToHtmlString();
            actualToken.Should().Be("<strong>by <a href=\"url.com\">this</a> article</strong>");
        }
    }
}