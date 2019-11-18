using FluentAssertions;
using Markdown;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownTokenReader_Should
    {
        private TokenReader tokenReader;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            tokenReader = new TokenReader(MarkdownTokenUtilities.GetMarkdownTokenDescriptions());
        }

        [TestCase("abcd")]
        [TestCase("dcba")]
        public void TokenizeSimpleText_Correctly(string text)
        {
            var expectedTokens = new List<Token>
            {
                new Token(text, 0, TokenType.Text, 4)
            };

            var tokens = tokenReader.TokenizeText(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void TokenizeEmptyString_Correctly()
        {
            var text = "";

            var tokens = tokenReader.TokenizeText(text);

            tokens.Select(tkn => tkn.TokenType).Should().BeEmpty();
        }

        [Test]
        public void ThrowArgumentNullException_OnNullString()
        {
            Action act = () => tokenReader.TokenizeText(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void TokenizeTextWithEscapeSymbol_Correctly()
        {
            var text = @"a\bc";
            var expectedTokens = new List<Token>
            {
                new Token(text, 0, TokenType.Text, 1),
                new Token(text, 1, TokenType.Escape, 2),
                new Token(text, 3, TokenType.Text, 1)
            };

            var tokens = tokenReader.TokenizeText(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void TokenizeTextWithEmphasisTag_Correctly()
        {
            var text = @"_a_";
            var expectedTokens = new List<Token>
            {
                new Token(text, 0, TokenType.Emphasis, 1),
                new Token(text, 1, TokenType.Text, 1),
                new Token(text, 2, TokenType.Emphasis, 1)
            };

            var tokens = tokenReader.TokenizeText(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void TokenizeTextWithStrongTag_Correctly()
        {
            var text = @"__a__";
            var expectedTokens = new List<Token>
            {
                new Token(text, 0, TokenType.Strong, 2),
                new Token(text, 2, TokenType.Text, 1),
                new Token(text, 3, TokenType.Strong, 2)
            };

            var tokens = tokenReader.TokenizeText(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void TokenizeTextWithDigits_Correctly()
        {
            var text = "ab12";
            var expectedTokens = new List<Token>
            {
                new Token(text, 0, TokenType.Text, 2),
                new Token(text, 2, TokenType.Digits, 2)
            };

            var tokens = tokenReader.TokenizeText(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void TokenizeTextWithSpace_Correctly()
        {
            var text = "ab cd";
            var expectedTokens = new List<Token>
            {
                new Token(text, 0, TokenType.Text, 2),
                new Token(text, 2, TokenType.WhiteSpace, 1),
                new Token(text, 3, TokenType.Text, 2)
            };

            var tokens = tokenReader.TokenizeText(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void TokenizeTextWithTokenAtStart_Correctly()
        {
            var text = "_ab";
            var expectedTokens = new List<Token>
            {
                new Token(text, 0, TokenType.Emphasis, 1),
                new Token(text, 1, TokenType.Text, 2)
            };

            var tokens = tokenReader.TokenizeText(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}
