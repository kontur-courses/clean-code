using FluentAssertions;
using Markdown.Handlers;
using NUnit.Framework;

namespace Markdown.Tests.Parsers
{
    [TestFixture]
    public class BoldTagMdParser_Test
    {
        private BoldTagMdParser sut;

        [OneTimeSetUp]
        public void StartTests()
        {
            sut = new BoldTagMdParser();
        }
        
        [Test]
        public void SelfTag_ShouldBe_DoubleUnderscore()
        {
            BoldTagMdParser.Tag.Should().Be(MdTags.Bold);
        }

        [TestCase("_italic text_")]
        [TestCase("# Heading")]
        [TestCase("default text")]
        [TestCase("_")]
        public void TryHandleTag_ShouldReturnNull_WhenAnotherTag(string textWithIncorrectTag)
        {
            sut.TryParseTag(0, textWithIncorrectTag)
                .Should().BeNull();
        }

        [TestCase("__bold text__", 0, 13)]
        [TestCase("__bold__ text", 0, 8)]
        [TestCase("__bold_ text__", 0, 14)]
        [TestCase("text __bold__", 5, 8)]
        [TestCase("__bo__ld", 0, 6)]
        [TestCase("b__ol__d", 1, 6)]
        [TestCase("bo__ld__", 2, 6)]
        public void TryHandleTag_ShouldReturnToken_WhenBoldTag(
            string textWithBoldSubtext, int startPosition, int subtextLengthWithTags)
        {
            var actualToken = sut.TryParseTag(startPosition, textWithBoldSubtext);
            actualToken.Should().NotBeNull();
            actualToken.Length.Should().Be(subtextLengthWithTags);
            actualToken.Position.Should().Be(startPosition);
            actualToken.Type.Should().Be(TextType.Bold);
            actualToken.Tag.Should().Be(MdTags.Bold);
        }

        [TestCase("__ text__", 0)]
        [TestCase("__text __", 0)]
        [TestCase("____", 0)]
        [TestCase("__12__3", 0)]
        [TestCase("wo__rd wo__rd", 2)]
        [TestCase("__Only open tag", 0)]
        public void TryHandleTag_ShouldReturnNull_WhenIncorrectTagUsing(
            string text, int startPosition)
        {
            sut.TryParseTag(startPosition, text)
                .Should().BeNull();
        }

        [Test]
        public void TryHandleTag_ShouldReturnNull_WhenShieldedCloseTag()
        {
            const string text = "__text\\__";
            sut.TryParseTag(0, text)
                .Should().BeNull();
        }

        [Test]
        public void TryHandleTag_ShouldReturnTag_WithCorrectValue()
        {
            const string text = "text __with bold__ subtext, and w__or__d";
            sut.TryParseTag(5, text)
                .GetValue(type => new Tag("", ""), text)
                .Should().Be("with bold");
            sut.TryParseTag(33, text)
                .GetValue(type => new Tag("", ""), text)
                .Should().Be("or");
        }
    }
}