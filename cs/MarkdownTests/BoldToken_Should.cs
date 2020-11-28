using System;
using FluentAssertions;
using Markdown.TokenModels;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class BoldToken_Should
    {
        [Test]
        public void Create_SimpleBoldToken_ReturnsCorrectToken() =>
            GetNewBoldTokenAsHtml("__some__", 0).Should().Be("<strong>some</strong>");

        [Test]
        public void Create_ItalicTokenInsideBoldToken_ReturnsCorrectBoldTokenWithItalicInside() =>
            GetNewBoldTokenAsHtml("__some _beautiful_ text__", 0)
                .Should()
                .Be("<strong>some <em>beautiful</em> text</strong>");

        [TestCase("__some")]
        [TestCase("some__")]
        [TestCase("__some __")]
        [TestCase("____")]
        public void Create_SomeIncorrectInputs_ReturnsNull(string rawToken)
        {
            Action callCreate = () => _ = new BoldToken(rawToken, 0);
            callCreate.Should().Throw<ArgumentException>();
        }

        [TestCase("__some__", 0, 8)]
        [TestCase("foo __bar__", 4, 7)]
        [TestCase("some __wrapped__ text", 5, 11)]
        public void GetLength_SomeItalicTokens_ReturnsCorrectInt(string rawToken, int startIndex, int expected) =>
            new BoldToken(rawToken, startIndex).MdTokenLength.Should().Be(expected);

        [Test]
        public void Create_HasTagsInDifferentWords_ReturnsNull()
        {
            Action callCreate = () => _ = new BoldToken("h__as whit__espace", 1);
            callCreate.Should().Throw<ArgumentException>();
        }

        private static string GetNewBoldTokenAsHtml(string input, int startIndex) =>
            new BoldToken(input, startIndex).ToHtmlString();
    }
}