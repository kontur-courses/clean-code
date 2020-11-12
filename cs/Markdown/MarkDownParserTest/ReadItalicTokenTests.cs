using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class ReadItalicTokenTests
    {
        [Test]
        public void ReadItalicToken_ShouldReturnTokenNeededType()
        {
            var token = MarkdownParser.ReadItalicToken("_a_", 0);
            token.Type.Should().Be(TokenType.Italic);
        }
        
        [Test]
        public void ReadItalicToken_ShouldReturnToken_WithGivenStartIndex()
        {
            var token = MarkdownParser.ReadItalicToken("_a_ _asdf_", 4);
            token.StartIndex.Should().Be(4);
        }

        [Test]
        public void ReadItalicToken_ShouldReturnTokenWithCorrectLength()
        {
            var token = MarkdownParser.ReadItalicToken("_asdf_alfk", 0);
            token.Length.Should().Be(6);
        }

        [Test]
        public void ReadItalicToken_ShouldReturnEmptyToken_OnBolderText()
        {
            var token = MarkdownParser.ReadItalicToken("__asdf__", 0);
            token.Should().BeEquivalentTo(Token.Empty());
        }

        [Test]
        public void ReadItalicToken_ShouldReturnEmptyToken_OnUnderscoreAroundDigits()
        {
            var token = MarkdownParser.ReadItalicToken("_123_", 0);
            token.Should().BeEquivalentTo(Token.Empty());
        }

        [Test]
        public void ReadItalicToken_ShouldReturnEmptyToken_InWhitespaceAfterUnderscope()
        {
            var token = MarkdownParser.ReadItalicToken("_ asdf_", 0);
            token.Should().BeEquivalentTo(Token.Empty());
        }

        [Test]
        public void ReadItalicToken_IgnoreUnderscopeAfterWhitespace()
        {
            var token = MarkdownParser.ReadItalicToken("_asdf _asdf_", 0);
            token.Length.Should().Be(12);
        }

        [Test]
        public void ReadItalicToken_ReturnTokenWithCorrectValue()
        {
            var token = MarkdownParser.ReadItalicToken("_asdf_", 0);
            token.Value.Should().Be("<em>asdf</em>");
        }
    }
    
}