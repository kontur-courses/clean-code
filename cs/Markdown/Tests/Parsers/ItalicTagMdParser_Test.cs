using FluentAssertions;
using Markdown.Handlers;
using Markdown.Parsers;
using NUnit.Framework;

namespace Markdown.Tests.Parsers
{
    public class ItalicTagMdParser_Test
    {
        private ItalicTagMdParser sut;

        [OneTimeSetUp]
        public void StartTests()
        {
            sut = new ItalicTagMdParser();
        }
        
        [Test]
        public void SelfTag_ShouldBe_Underscore()
        {
            ItalicTagMdParser.Tag.Should().Be(MdTags.Italic);
        }

        [TestCase("# Heading")]
        [TestCase("default text")]
        [TestCase("**bold text**")]
        [TestCase("")]
        public void TryHandleTag_ShouldReturnNull_WhenAnotherTag(string textWithIncorrectTag)
        {
            sut.TryParseTag(0, textWithIncorrectTag)
                .Should().BeNull();
        }

        [TestCase("_italic text_", 0, 13)]
        [TestCase("_italic_ text", 0, 8)]
        [TestCase("text _italic_", 5, 8)]
        [TestCase("_ital_ic", 0, 6)]
        [TestCase("i_tali_c", 1, 6)]
        [TestCase("it_alic_", 2, 6)]
        public void TryHandleTag_ShouldReturnToken_WhenItalicTag(
            string textWithBoldSubtext, int startPosition, int subtextLengthWithTags)
        {
            var actualToken = sut.TryParseTag(startPosition, textWithBoldSubtext);
            actualToken.Should().NotBeNull();
            actualToken.Length.Should().Be(subtextLengthWithTags);
            actualToken.Position.Should().Be(startPosition);
            actualToken.Type.Should().Be(TextType.Italic);
            actualToken.Tag.Should().Be(MdTags.Italic);
        }
        
        [Test]
        public void TryHandleTag_ShouldReturnNull_WhenShieldedCloseTag()
        {
            const string text = "_text\\_";
            sut.TryParseTag(0, text)
                .Should().BeNull();
        }

        [TestCase("_ text_", 0)]
        [TestCase("_text _", 0)]
        [TestCase("__", 0)]
        [TestCase("_12_3", 0)]
        [TestCase("wo_rd wo_rd", 2)]
        [TestCase("_Only open tag", 0)]
        public void TryHandleTag_ShouldReturnNull_WhenIncorrectTagUsing(
            string text, int startPosition)
        {
            sut.TryParseTag(startPosition, text)
                .Should().BeNull();
        }

        [Test]
        public void TryHandleTag_ShouldReturnTag_WithCorrectValue()
        {
            const string text = "text _with italic_ subtext, and w_or_d";
            sut.TryParseTag(5, text)
                .GetValue(type => new Tag("", ""), text)
                .Should().Be("with italic");
            sut.TryParseTag(33, text)
                .GetValue(type => new Tag("", ""), text)
                .Should().Be("or");
        }
    }
}