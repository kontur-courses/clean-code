using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Lexer
{
    [TestFixture]
    public class Tokenizer_Should
    {
        [TestCase("", Description = "Empty string")]
        [TestCase("foo bar baz", TokenType.Text, Description = "There are not tags")]
        [TestCase("_foo_", TokenType.OpeningTag, TokenType.Text, TokenType.ClosingTag,
            Description = "Text between single underscores")]
        [TestCase("__foo__", TokenType.OpeningTag, TokenType.Text, TokenType.ClosingTag,
            Description = "Text between double underscores")]
        [TestCase("foo _bar_ baz",
            TokenType.Text, TokenType.OpeningTag, TokenType.Text, TokenType.ClosingTag, TokenType.Text,
            Description = "Tags between text")]
        [TestCase("__foo _bar_ baz__",
            TokenType.OpeningTag, TokenType.Text, TokenType.OpeningTag, TokenType.Text,
            TokenType.ClosingTag, TokenType.Text, TokenType.ClosingTag,
            Description = "Nested tags")]
        [TestCase("foo_1_2", TokenType.Text, Description = "Underscore with digits")]
        [TestCase("_ foo", TokenType.Text, Description = "Space after opening tag")]
        [TestCase("_bar _ ", TokenType.OpeningTag, TokenType.Text, Description = "Space before closing tag")]
        public void GetNext_ReturnsCorrectTokenTypes(string text, params TokenType[] types)
        {
            var tokenizer = new Tokenizer();

            var tokens = tokenizer.GetTokens(text);

            tokens.Select(token => token.Type).Should().BeEquivalentTo(types);
        }

        [Test]
        public void GetNext_ReturnsText_WhenLexemeIsEscaped()
        {
            const string text = @"foo \_bar\_ baz";
            var tokenizer = new Tokenizer();
            var expected = text.Replace(@"\", "");

            var tokens = tokenizer.GetTokens(text);
            
            tokens.Select(t => t.Value).Should().BeEquivalentTo(expected);
        }
    }
}