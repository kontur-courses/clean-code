using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class LinkToken_Test
    {
        private LinkToken sut;
        
        [Test]
        public void LinkPosition_ShouldBe_SavedFromConstructor()
        {
            const int position = 3;
            sut = new LinkToken(0, 10, MdTags.Link, TextType.Link, position, "");
            sut.LinkPosition.Should().Be(position);
        }
        
        [Test]
        public void Link_ShouldBe_SavedFromConstructor()
        {
            const string link = "link.com";
            sut = new LinkToken(0, 10, MdTags.Link, TextType.Link, 0, link);
            sut.Link.Should().Be(link);
        }

        [Test]
        public void GetValue_ShouldReturn_OnlyInternalText_WhenBadTransformFunction()
        {
            const string text = "[text](link.com)";
            sut = new LinkToken(0, 16, MdTags.Link, TextType.Link, 7, "link.com");
            sut.GetValue(token => MdTags.Default, text).Should().Be("text");
        }
    }
}