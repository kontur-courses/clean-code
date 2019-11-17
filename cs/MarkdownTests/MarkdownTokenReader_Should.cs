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
            var expectedTexts = new List<string>() { text};
            var expectedTypes = new List<TokenType>() { TokenType.Text};
            var expectedLengths = new List<int>() { 4 };
            var expectedPositions = new List<int>() { 0};

            var tokens = tokenReader.TokenizeText(text);

            CheckTokenizationValidity(tokens,
                expectedTexts, expectedTypes, expectedLengths, expectedPositions);
        }

        [Test]
        public void TokenizeEmptyString_Correctly()
        {
            var text = "";
            var expectedTokenTypes = new List<TokenType>() { };

            var tokens = tokenReader.TokenizeText(text);

            tokens.Select(tkn => tkn.TokenType).Should().BeEquivalentTo(expectedTokenTypes);
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
            var expectedTexts = new List<string>() { "a", @"\b", "c"};
            var expectedTypes = new List<TokenType>()
                { TokenType.Text, TokenType.Escape, TokenType.Text};
            var expectedLengths = new List<int>() { 1, 2, 1};
            var expectedPositions = new List<int>() { 0, 1, 3};

            var tokens = tokenReader.TokenizeText(text);

            CheckTokenizationValidity(tokens,
                expectedTexts, expectedTypes, expectedLengths, expectedPositions);
        }

        [Test]
        public void TokenizeTextWithEmphasisTag_Correctly()
        {
            var text = @"_a_";
            var expectedTexts = new List<string>() { "_", "a", "_"};
            var expectedTypes = new List<TokenType>()
                { TokenType.Emphasis, TokenType.Text, TokenType.Emphasis};
            var expectedLengths = new List<int>() { 1, 1, 1};
            var expectedPositions = new List<int>() { 0, 1, 2};

            var tokens = tokenReader.TokenizeText(text);

            CheckTokenizationValidity(tokens,
                expectedTexts, expectedTypes, expectedLengths, expectedPositions);
        }

        [Test]
        public void TokenizeTextWithStrongTag_Correctly()
        {
            var text = @"__a__";
            var expectedTexts = new List<string>() { "__", "a", "__"};
            var expectedTypes = new List<TokenType>()
                { TokenType.Strong, TokenType.Text, TokenType.Strong};
            var expectedLengths = new List<int>() { 2, 1, 2};
            var expectedPositions = new List<int>() { 0, 2, 3};

            var tokens = tokenReader.TokenizeText(text);

            CheckTokenizationValidity(tokens,
                expectedTexts, expectedTypes, expectedLengths, expectedPositions);
        }

        [Test]
        public void TokenizeTextWithDigits_Correctly()
        {
            var text = "ab12";
            var expectedTexts = new List<string>() { "ab", "12"};
            var expectedTypes = new List<TokenType>()
                { TokenType.Text, TokenType.Digits};
            var expectedLengths = new List<int>() { 2, 2};
            var expectedPositions = new List<int>() { 0, 2};

            var tokens = tokenReader.TokenizeText(text);

            CheckTokenizationValidity(tokens,
                expectedTexts, expectedTypes, expectedLengths, expectedPositions);
        }

        [Test]
        public void TokenizeTextWithSpace_Correctly()
        {
            var text = "ab cd";
            var expectedTexts = new List<string>() { "ab", " ", "cd"};
            var expectedTypes = new List<TokenType>()
                { TokenType.Text, TokenType.WhiteSpace, TokenType.Text};
            var expectedLengths = new List<int>() { 2, 1, 2};
            var expectedPositions = new List<int>() { 0, 2, 3};

            var tokens = tokenReader.TokenizeText(text);

            CheckTokenizationValidity(tokens,
                expectedTexts, expectedTypes, expectedLengths, expectedPositions);
        }

        [Test]
        public void TokenizeTextWithTokenAtStart_Correctly()
        {
            var text = "_ab";
            var expectedTexts = new List<string>() { "_", "ab"};
            var expectedTypes = new List<TokenType>()
                { TokenType.Emphasis, TokenType.Text};
            var expectedLengths = new List<int>() { 1, 2};
            var expectedPositions = new List<int>() { 0, 1};

            var tokens = tokenReader.TokenizeText(text);

            CheckTokenizationValidity(tokens, 
                expectedTexts, expectedTypes, expectedLengths, expectedPositions);
        }

        public void CheckTokenizationValidity(IEnumerable<Token> tokens,
            IEnumerable<string> expectedTexts, IEnumerable<TokenType> expectedTypes,
            IEnumerable<int> expectedLength, IEnumerable<int> expectedPositions)
        {
            tokens.Select(tkn => tkn.TokenType).Should().BeEquivalentTo(expectedTypes,
                "token types should be equal");
            tokens.Select(tkn => tkn.Length).Should().BeEquivalentTo(expectedLength, 
                "lengths should be equal");
            tokens.Select(tkn => tkn.Position).Should().BeEquivalentTo(expectedPositions,
                "positions should be equal");
            tokens.Select(tkn => tkn.Text).Should().BeEquivalentTo(expectedTexts,
                "texts should be equal");
        }
    }
}
