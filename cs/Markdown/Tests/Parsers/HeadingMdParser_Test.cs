using FluentAssertions;
using Markdown.Parsers;
using NUnit.Framework;

namespace Markdown.Tests.Parsers
{
    [TestFixture]
    public class HeadingMdParser_Test
    {
        private HeadingMdParser sut;
        
        [OneTimeSetUp]
        public void StartTests()
        {
            sut = new HeadingMdParser();
        }

        [Test]
        public void Tag_ShouldBe_NumberSign()
        {
            HeadingMdParser.Tag.Should().Be(MdTags.Heading);
        }
        
        [TestCase("__bold__", 0)]
        [TestCase("_italic_", 0)]
        [TestCase("text", 0)]
        [TestCase("\ntext", 1)]
        [TestCase("", 1)]
        public void TryHandleTag_ShouldReturnNull_WhenNotHeading(string text, int position)
        {
            sut.TryParseTag(position, text).Should().BeNull();
        }

        [Test]
        public void TryHandleTag_ShouldReturnShortToken_WhenTextHasNotEndl_AndStartsFromZeroPosition()
        {
            const string text = "# Heading";
            var token = sut.TryParseTag(0, text);
            token.Should().NotBeNull();
            token.Position.Should().Be(0);
            token.Length.Should().Be(text.Length);
            token.Type.Should().Be(TextType.Heading);
        }
        
        [Test]
        public void TryHandleTag_ShouldReturnShortToken_WhenTextHasNotEndl_AndStartsAfterEndl()
        {
            const string text = "text\n# Heading";
            var token = sut.TryParseTag(5, text);
            token.Should().NotBeNull();
            token.Position.Should().Be(5);
            token.Length.Should().Be(9);
            token.Type.Should().Be(TextType.Heading);
        }
        
        [Test]
        public void TryHandleTag_ShouldReturnShortToken_WhenTextHasEndl_AndStartsAfterEndl()
        {
            const string text = "text\n# Heading text\ntext";
            var token = sut.TryParseTag(5, text);
            token.Should().NotBeNull();
            token.Position.Should().Be(5);
            token.Length.Should().Be(14);
            token.Type.Should().Be(TextType.Heading);
        }

        [TestCase("#Not heading", 0)]
        [TestCase("text # Not heading", 5)]
        public void TryHandleTag_ShouldReturnNull_WhenIncorrectTagUsing(string text, int position)
        {
            sut.TryParseTag(position, text).Should().BeNull();
        }
    }
}