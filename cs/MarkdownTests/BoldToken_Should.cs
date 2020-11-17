using FluentAssertions;
using Markdown.TokenModels;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class BoldToken_Should
    {
        [Test]
        public void Create_SimpleBoldToken_ReturnsCorrectToken()
        {
            var actual = BoldToken.Create("__some__", 0).ToHtmlString();
            const string expected = "<strong>some</strong>";

            actual.Should().Be(expected);
        }

        [Test]
        public void Create_ItalicTokenInsideBoldToken_ReturnsCorrectBoldTokenWithItalicInside()
        {
            var actual = BoldToken.Create("__some _beautiful_ text__", 0).ToHtmlString();
            const string expected = "<strong>some <em>beautiful</em> text</strong>";

            actual.Should().Be(expected);
        }

        [TestCase("__some")]
        [TestCase("some__")]
        [TestCase("__some __")]
        public void Create_SomeIncorrectInputs_ReturnsNull(string rawToken) =>
            BoldToken.Create(rawToken, 0).Should().BeNull();

        [TestCase("__some__", 0, 8)]
        [TestCase("foo __bar__", 4, 7)]
        [TestCase("some __wrapped__ text", 5, 11)]
        public void GetLength_SomeItalicTokens_ReturnsCorrectInt(string rawToken, int startIndex, int expected) =>
            BoldToken.Create(rawToken, startIndex).MdTokenLength.Should().Be(expected);
    }
}