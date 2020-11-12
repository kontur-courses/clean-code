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
                .BeEquivalentTo(new Token(5, 0, TokenType.Header, "asdf"));
        }

        [Test]
        public void ReadHeadToken_ReturnCorrectToken_WithInsertedItalicToken()
        {
            var token = MarkdownParser.ReadHeaderToken("#one _two_ three");
            var inserted = new Token[] { new Token(5, 5, TokenType.Italic, "two")};
            token.InsertedTokens.Should().BeEquivalentTo(inserted);
        }

        [Test]
        public void ReadHeadToken_ReturnCorrectToken_WithInsertedBoldToken()
        {
            var token = MarkdownParser.ReadHeaderToken("#one __two__ three");
            var inserted = new Token[] {new Token(7, 5, TokenType.Bold, "two")};
            token.InsertedTokens.Should().BeEquivalentTo(inserted);
        }
        
    }
}