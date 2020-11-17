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
        public void Create_SomeIncorrectInputs_ReturnsNull(string rawToken) =>
            BoldToken.Create(rawToken, 0).Should().BeNull();

        [TestCase("__some__", 0, 8)]
        [TestCase("foo __bar__", 4, 7)]
        [TestCase("some __wrapped__ text", 5, 11)]
        public void GetLength_SomeItalicTokens_ReturnsCorrectInt(string rawToken, int startIndex, int expected) =>
            BoldToken.Create(rawToken, startIndex).MdTokenLength.Should().Be(expected);
        
        private static string GetNewBoldTokenAsHtml(string input, int startIndex) =>
            BoldToken.Create(input, startIndex).ToHtmlString();
    }
}