using FluentAssertions;
using Markdown.Parsers;
using NUnit.Framework;

namespace Markdown.Tests.Parsers
{
    [TestFixture]
    public class LinkMdParser_Test
    {
        private LinkMdParser sut;
        
        [OneTimeSetUp]
        public void StartTests()
        {
            sut = new LinkMdParser();
        }

        [Test]
        public void Tag_ShouldBe_OpenSquareBracket_AndCloseCircleBracket()
        {
            LinkMdParser.Tag.Should().Be(MdTags.Link);
        }

        [TestCase("text", 0)]
        [TestCase("_text_", 0)]
        [TestCase("# text", 0)]
        public void TryParseTag_ShouldReturnNull_WhenNotLinkTag(string text, int position)
        {
            sut.TryParseTag(position, text).Should().BeNull();
        }
        
        [TestCase("text](link)", 0)]
        [TestCase("text(link)", 0)]
        [TestCase("[text]link", 0)]
        [TestCase("[text](link", 0)]
        [TestCase("[text(link)", 0)]
        [TestCase("[text]()", 0)]
        [TestCase("[](link)", 0)]
        [TestCase("[text]", 0)]
        [TestCase("[", 0)]
        public void TryParseTag_ShouldReturnNull_WhenIncorrectTagUsing(string text, int position)
        {
            sut.TryParseTag(position, text).Should().BeNull();
        }
        
        [TestCase("[text](link.ru)", 0, 7)]
        [TestCase("[text inside](link link.ru)", 0, 14)]
        [TestCase("su[bwo](link.ru)rd link", 2, 8)]
        public void TryParseTag_ShouldReturnToken_WhenLinkTag(string text, int position, int linkPosition)
        {
            var token = sut.TryParseTag(position, text) as LinkToken;
            token.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            token.Position.Should().Be(position);
            token.LinkPosition.Should().Be(linkPosition);
            token.Type.Should().Be(TextType.Link);
        }
    }
}