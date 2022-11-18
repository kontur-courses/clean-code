using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class HtmlWriter_Test
    {
        [Test]
        public void CreateHtmlFromTokens_ShouldCreateString_WithBoldText()
        {
            const string text = "__bold__ text";
            const string expectedText = "<strong>bold</strong> text";
            var rootToken = new Token(0, 13, MdTags.Default, TextType.Default);
            var boldToken = new Token(0, 8, MdTags.Bold, TextType.Bold);
            rootToken.AddInternalToken(boldToken);
            HtmlWriter.CreateHtmlFromTokens(rootToken, text).Should().Be(expectedText);
        }

        [Test]
        public void CreateHtmlFromTokens_ShouldCreateString_WithItalicText()
        {
            const string text = "_italic_ text";
            const string expectedText = "<em>italic</em> text";
            var rootToken = new Token(0, 13, MdTags.Default, TextType.Default);
            var italicToken = new Token(0, 8, MdTags.Italic, TextType.Italic);
            rootToken.AddInternalToken(italicToken);
            HtmlWriter.CreateHtmlFromTokens(rootToken, text).Should().Be(expectedText);
        }

        [Test]
        public void CreateHtmlFromTokens_ShouldCreateString_WithHeading()
        {
            const string text = "# Heading\nText";
            const string expectedText = "<h1>Heading</h1>\nText";
            var rootToken = new Token(0, 14, MdTags.Default, TextType.Default);
            var headingToken = new Token(0, 9, MdTags.Heading, TextType.Heading);
            rootToken.AddInternalToken(headingToken);
            HtmlWriter.CreateHtmlFromTokens(rootToken, text).Should().Be(expectedText);
        }
        
        [Test]
        public void CreateHtmlFromTokens_ShouldCreateString_WithBoldItalicHeading()
        {
            const string text = "# __Bold _italic_ heading__ _with italic_\nText";
            const string expectedText = "<h1><strong>Bold <em>italic</em> heading</strong> <em>with italic</em></h1>\nText";
            var rootToken = new Token(0, 46, MdTags.Default, TextType.Default);
            var headingToken = new Token(0, 41, MdTags.Heading, TextType.Heading);
            var boldToken = new Token(2, 25, MdTags.Bold, TextType.Bold);
            var boldItalicToken = new Token(9, 8, MdTags.Italic, TextType.Italic);
            var italicToken = new Token(28, 13, MdTags.Italic, TextType.Italic);
            rootToken.AddInternalToken(headingToken);
            headingToken.AddInternalToken(boldToken);
            headingToken.AddInternalToken(italicToken);
            boldToken.AddInternalToken(boldItalicToken);
            HtmlWriter.CreateHtmlFromTokens(rootToken, text).Should().Be(expectedText);
        }
    }
}