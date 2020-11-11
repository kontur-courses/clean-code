using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    [TestFixture]
    public class ReadBoldTokenTests
    {
        [Test]
        public void ReadBoldToken_ReturnTokenNeededType()
        {
            var token = MarkdownParser.ReadBoldToken("__asdf__", 0);
            token.Type.Should().Be(TokenType.Bold);
        }

        [Test]
        public void ReadBoldToken_ReturnEmptyToken_onStringWithoutUnderscopes()
        {
            var token = MarkdownParser.ReadBoldToken("asdf", 0);
            token.Should().BeEquivalentTo(Token.Empty());
        }

        [Test]
        public void ReadBoldToken_ReturnTokenWithCorrectStartIndex()
        {
            var token = MarkdownParser.ReadBoldToken("asdf __asdf__", 5);
            token.StartIndex.Should().Be(5);
        }

        [Test]
        public void ReadBoldToken_ReturnTokenCorrectLength()
        {
            var token = MarkdownParser.ReadBoldToken("a __asdf__", 2);
            token.Length.Should().Be(8);
        }

        [Test]
        public void ReadBoldToken_ReturnEmptyToken_WhenWhiteSpaceAfterUnderscopes()
        {
            var token = MarkdownParser.ReadBoldToken("__ asdf__", 0);
            token.Should().BeEquivalentTo(Token.Empty());
        }

        [Test]
        public void ReadBoldToken_ShouldIgnoreUnderscopesAfterWhitespace()
        {
            var token = MarkdownParser.ReadBoldToken("__asdf __asdf", 0);
            token.Should().BeEquivalentTo(Token.Empty());
        }

        [Test]
        public void ReadBoldToken_ShouldReturnToken_WithCorrectValue()
        {
            var token = MarkdownParser.ReadBoldToken("__asdf__", 0);
            token.Value.Should().Be("asdf");
        }

        [Test]
        public void ReadBoldToken_ShouldReturnToken_WithInsertedItalicToken()
        {
            var token = MarkdownParser.ReadBoldToken("__asdf _one_ asdf__", 0);
            token.InsertedTokens[0].Should()
                .BeEquivalentTo(new Token(5, 7, TokenType.Italic, "one"));
        }

        [Test]
        public void ReadBoldToken_ShouldReturnToken_WithManyInsertedItalicTokens()
        {
            var token = MarkdownParser.ReadBoldToken("__asdf _one_ _two_ asdf__", 0);
            token.InsertedTokens.Count.Should().Be(2);
            token.InsertedTokens[0].Should()
                .BeEquivalentTo(new Token(5, 7, TokenType.Italic, "one"));
            token.InsertedTokens[1].Should()
                .BeEquivalentTo(new Token(5, 13, TokenType.Italic, "two"));
        }

    }
}