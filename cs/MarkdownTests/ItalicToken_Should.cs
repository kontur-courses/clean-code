using FluentAssertions;
using Markdown.TokenModels;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class ItalicToken_Should
    {
        [Test]
        public void Create_SimpleItalicToken_ReturnsCorrectToken()
        {
            var actual = ItalicToken.Create("_some_", 0).ToHtmlString();
            const string expected = "<em>some</em>";
            actual.Should().Be(expected);
        }

        [Test]
        public void Create_ItalicTokenAfterStringToken_ReturnsCorrect()
        {
            var actual = ItalicToken.Create("foo _some bar_", 4).ToHtmlString();
            const string expected = "<em>some bar</em>";
            actual.Should().Be(expected);
        }

        [Test]
        public void Create_BoldTokenIntoItalicToken_ReturnsOnlyItalicToken()
        {
            var actual = ItalicToken.Create("_some __beautiful__ text_", 0).ToHtmlString();
            const string expected = "<em>some __beautiful__ text</em>";
            actual.Should().Be(expected);
        }

        [TestCase("_some")]
        [TestCase("some_")]
        [TestCase("_some _")]
        public void Create_SomeIncorrectInputs_ReturnsNull(string rawToken) =>
            ItalicToken.Create(rawToken, 0).Should().BeNull();

        [TestCase("_some_", 0, 6)]
        [TestCase("foo _bar_", 4, 5)]
        [TestCase("some _wrapped_ text", 5, 9)]
        public void GetMdTokenLength_SomeItalicTokens_ReturnsCorrectInt(string rawToken, int startIndex,
            int expected) =>
            ItalicToken.Create(rawToken, startIndex).MdTokenLength.Should().Be(expected);
    }
}