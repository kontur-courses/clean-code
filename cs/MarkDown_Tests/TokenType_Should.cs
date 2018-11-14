using FluentAssertions;
using MarkDown;
using MarkDown.TagTypes;
using NUnit.Framework;

namespace MarkDown_Tests
{
    [TestFixture]
    public class TokenType_Should
    {
        private Token tagToken;
        private Token textToken;

        [SetUp]
        public void SetUp()
        {
            tagToken = new Token(0, "aaa", new EmTag());
            textToken = new Token(0, "aaa");
        }

        [Test]
        public void HaveCorrectPosition_AfterCreation_OnTagToken() => tagToken.Position.Should().Be(0);

        [Test]
        public void HaveCorrectTagType_AfterCreation_OnTagToken() => 
            tagToken.TagType.GetType().Should().Be(new EmTag().GetType());

        [Test]
        public void HaveCorrectTokenType_AfterCreation_OnTagToken() => tagToken.TokenType.Should().Be(TokenType.Tag);

        [Test]
        public void HaveCorrectContent_AfterCreation_OnTagToken() => tagToken.Content.Should().Be("aaa");

        [Test]
        public void HaveCorrectLength_AfterCreation_OnTagToken() => tagToken.Length.Should().Be(5);

        [Test]
        public void HaveCorrectPosition_AfterCreation_OnTextToken() => textToken.Position.Should().Be(0);

        [Test]
        public void HaveCorrectTagType_AfterCreation_OnTextToken() => textToken.TagType.Should().BeNull();

        [Test]
        public void HaveCorrectTokenType_AfterCreation_OnTextToken() => textToken.TokenType.Should().Be(TokenType.Text);

        [Test]
        public void HaveCorrectContent_AfterCreation_OnTextToken() => textToken.Content.Should().Be("aaa");

        [Test]
        public void HaveCorrectLength_AfterCreation_OnTextToken() => textToken.Length.Should().Be(3);
    }
}
