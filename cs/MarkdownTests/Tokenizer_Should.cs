using System.Collections.Generic;
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
            actual.Should().BeEquivalentTo(new StringToken("some").ToHtmlString());
        }

        [Test]
        public void ParseIntoTokens_PlainMdItalicText_ReturnsCorrectItalicToken()
        {
            const string mdText = "_some_";

            var expectedToken = ItalicToken.Create("some").ToHtmlString();
            var actualToken = Tokenizer.ParseIntoTokens(mdText).First().ToHtmlString();

            actualToken.Should().BeEquivalentTo(expectedToken);
        }
        
        [Test]
        public void ParseIntoTokens_PlainMdHeaderText_ReturnsCorrectHeaderToken()
        {
            const string mdText = "# some";

            var expectedToken = HeaderToken.Create("some").ToHtmlString();
            var actualToken = Tokenizer.ParseIntoTokens(mdText).First().ToHtmlString();

            actualToken.Should().BeEquivalentTo(expectedToken);
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
            const string expectedTokenAsHtml = "<h1><strong>some <em>meaningful</em> text</strong></h1>\n";
            
            var actualToken = Tokenizer.ParseIntoTokens(mdText).First();
            actualToken.ToHtmlString().Should().Be(expectedTokenAsHtml);
        }
    }
}