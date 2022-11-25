using FluentAssertions;
using Markdown.HtmlTag;
using Markdown.Markdown;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class ExceptionHandlingShould
    {
        [TestCase("", TestName = "string is \"\"")]
        [TestCase(null, TestName = "string is null")]
        public void ThrowException_WhenRenderEmptyString(string nullString)
        {
            Action resultAction = () => Md.Render(nullString);
            resultAction.Should().Throw<ArgumentNullException>();
        }

        [TestCase("", TestName = "parse string is \"\"")]
        [TestCase(null, TestName = "parse string is null")]
        public void ThrowException_WhenParseMarkdownTokenStringIsNullOrEmpty(string nullString)
        {
            Action resultAction = () => MarkdownParser.GetArrayWithMdTags(nullString);
            resultAction.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ThrowException_WhenTokenParserHaveEmptyToken()
        {
            var tokens = new List<Token>() { new Token(0, 2), new Token(2, 0) };
            Action resultAction = () => TokenParser.GetTokens(tokens.ToArray(), 2);
            resultAction.Should().Throw<ArgumentNullException>();
        }

        [TestCase(0, TestName = "Index is 0")]
        [TestCase(-10, TestName = "Index negative")]
        public void ThrowException_WhenTokenParserHaveNotEndIndex(int endIndex)
        {
            var tokens = new List<Token>() { new Token(0, 2), new Token(2, 2) };
            Action resultAction = () => TokenParser.GetTokens(tokens.ToArray(), endIndex);
            resultAction.Should().Throw<ArgumentException>();
        }

        [TestCase("some text for check without tags", TestName = "simple text")]
        [TestCase("12 12321343 4534243", TestName = "digits text")]
        [TestCase("               ", TestName = "whitespaces text")]
        [TestCase("__ _ _____", TestName = "tags text")]
        public void NotThrowExceptions_WhenRenderStringHaveNotTagsOrHaveNotWords(string markdownString)
        {
            Action resultAction = () => Md.Render(markdownString);
            resultAction.Should().NotThrow<Exception>();
        }

        [Test]
        public void ThrowException_WhenTokenFilterIsEmpty()
        {
            Action resultAction = () => TokenFilter.Filter(new List<Token>(), "");
            resultAction.Should().Throw<ArgumentException>();
        }

        [Test]
        public void ThrowException_WhenTokenPositionIsNegative()
        {
            Action resultAction = () => new Token(-1, 1);
            resultAction.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void ThrowException_WhenStringForParseHaveNotAllTokens()
        {
            Action resultAction = () => HtmlParser.Parse(new List<Token>() { new Token(0, 2), new Token(2, 3) }, "__FAFf");
            resultAction.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestCase("", TestName = "Html tag is \"\"")]
        [TestCase(null, TestName = "HTML tag is null")]
        public void ThrowException_WhenHtmlTagIsEmpty(string htmlTag)

        {
            Action resultAction = () => new HtmlTag.HtmlTag(htmlTag);
            resultAction.Should().Throw<ArgumentNullException>();
        }

        [TestCase("", "", TestName = "Html tags is \"\"")]
        [TestCase(null, null, TestName = "HTML tags is null")]
        public void ThrowException_WhenHtmlTagDifferentElementsIsEmpty(string startTag, string endTag)
        {
            Action resultAction = () => new HtmlTag.HtmlTag(startTag, endTag);
            resultAction.Should().Throw<ArgumentNullException>();
        }
    }
}
