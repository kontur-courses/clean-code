using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class ReadHeaderTokenTests
    {
        [Test]
        public void ReadHeaderToken_ReturnEmptyToken_OnNotHeaderString()
        {
            var token = MarkdownParser.ReadHeaderToken("asdf");
            token.Should().BeEquivalentTo(Token.Empty());
        }

        [Test]
        public void ReadHeaderToken_ReturnCorrectToken_OnSimpleHeader()
        {
            var token = MarkdownParser.ReadHeaderToken("#asdf");
            token.Should()
                .BeEquivalentTo(new Token(5, 0, TokenType.Header, "#asdf"));
        }

    }
}