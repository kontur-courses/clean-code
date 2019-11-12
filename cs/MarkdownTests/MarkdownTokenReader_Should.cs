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
            tokenReader = new TokenReader(MarkdownUtilities.GetMarkdownTokenDescriptions());
        }

        [TestCase("abcd")]
        [TestCase("dcba")]
        public void TokenizeSimpleText_Correctly(string text)
        {
            var tokens = tokenReader.TokenizeText(text);
            tokens.Select(tkn => tkn.GetTokenString()).Should().BeEquivalentTo(
                new List<string>() { text, "" });
            tokens.Select(tkn => tkn.TokenType).Should().BeEquivalentTo(
                new List<TokenType>() { TokenType.Text, TokenType.Eof });
        }

        [Test]
        public void TokenizeEmptyString_Correctly()
        {
            var tokens = tokenReader.TokenizeText("");
            tokens.Select(tkn => tkn.TokenType).Should().BeEquivalentTo(
                new List<TokenType>() { TokenType.Eof });
        }

        [Test]
        public void ThrowArgumentNullException_OnNullString()
        {
            Action act = () => tokenReader.TokenizeText(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void TokenizeTextWithEndOfLine_Correctly()
        {
            var text = "ab\ncd";
            var tokens = tokenReader.TokenizeText(text);
            tokens.Select(tkn => tkn.GetTokenString()).Should().BeEquivalentTo(
                new List<string>() { "ab", "\n", "cd", "" });
            tokens.Select(tkn => tkn.TokenType).Should().BeEquivalentTo(
                new List<TokenType>()
                { TokenType.Text, TokenType.Eol, TokenType.Text, TokenType.Eof });
        }

        [Test]
        public void TokenizeTextWithEscapeSymbol_Correctly()
        {
            var text = @"a\bc";
            var tokens = tokenReader.TokenizeText(text);
            tokens.Select(tkn => tkn.GetTokenString()).Should().BeEquivalentTo(
                new List<string>() { "a", @"\b", "c", "" });
            tokens.Select(tkn => tkn.TokenType).Should().BeEquivalentTo(
                new List<TokenType>()
                { TokenType.Text, TokenType.Escape, TokenType.Text, TokenType.Eof });
        }

        [Test]
        public void TokenizeTextWithEmphasisTag_Correctly()
        {
            var text = @"_a_";
            var tokens = tokenReader.TokenizeText(text);
            tokens.Select(tkn => tkn.GetTokenString()).Should().BeEquivalentTo(
                new List<string>() { "_", "a", "_", "" });
            tokens.Select(tkn => tkn.TokenType).Should().BeEquivalentTo(
                new List<TokenType>()
                { TokenType.Emphasis, TokenType.Text, TokenType.Emphasis, TokenType.Eof });
        }

        [Test]
        public void TokenizeTextWithStrongTag_Correctly()
        {
            var text = @"__a__";
            var tokens = tokenReader.TokenizeText(text);
            tokens.Select(tkn => tkn.GetTokenString()).Should().BeEquivalentTo(
                new List<string>() { "__", "a", "__", "" });
            tokens.Select(tkn => tkn.TokenType).Should().BeEquivalentTo(
                new List<TokenType>()
                { TokenType.Strong, TokenType.Text, TokenType.Strong, TokenType.Eof });
        }

        [Test]
        public void TokenizeTextWithDigits_Correctly()
        {
            var text = "ab12";
            var tokens = tokenReader.TokenizeText(text);
            tokens.Select(tkn => tkn.GetTokenString()).Should().BeEquivalentTo(
                new List<string>() { "ab", "12", "" });
            tokens.Select(tkn => tkn.TokenType).Should().BeEquivalentTo(
                new List<TokenType>()
                { TokenType.Text, TokenType.Digits, TokenType.Eof });
        }

        [Test]
        public void TokenizeTextWithSpace_Correctly()
        {
            var text = "ab cd";
            var tokens = tokenReader.TokenizeText(text);
            tokens.Select(tkn => tkn.GetTokenString()).Should().BeEquivalentTo(
                new List<string>() { "ab", " ", "cd", "" });
            tokens.Select(tkn => tkn.TokenType).Should().BeEquivalentTo(
                new List<TokenType>()
                { TokenType.Text, TokenType.WhiteSpace, TokenType.Text, TokenType.Eof });
        }

        [Test]
        public void TokenizeTextWithTokenAtStart_Correctly()
        {
            var text = "_ab";
            var tokens = tokenReader.TokenizeText(text);
            tokens.Select(tkn => tkn.GetTokenString()).Should().BeEquivalentTo(
                new List<string>() { "_", "ab", "" });
            tokens.Select(tkn => tkn.TokenType).Should().BeEquivalentTo(
                new List<TokenType>()
                { TokenType.Emphasis, TokenType.Text, TokenType.Eof });
        }
    }
}
