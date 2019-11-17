using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    class TokenReaderTests
    {
        private readonly TokenReader tokenReader = new TokenReader(new Md().MdTokenDescriptions);

        [Test]
        public void SplitToTokens_ShouldReturnCorrectTokensList_IfOnlyLetters()
        {
            var text = "abc";

            var tokensList = tokenReader.SplitToTokens(text);

            var expected = new List<Token> {new Token(0, "abc", TokenType.Letters)};
            tokensList.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SplitToTokens_ShouldReturnCorrectTokensList_IfOnlyNumber()
        {
            var text = "123";

            var tokensList = tokenReader.SplitToTokens(text);

            var expected = new List<Token> { new Token(0, "123", TokenType.Number) };
            tokensList.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SplitToTokens_ShouldReturnCorrectTokensList_IfOnlyWhiteSpaces()
        {
            var text = "   ";

            var tokensList = tokenReader.SplitToTokens(text);

            var expected = new List<Token> { new Token(0, "   ", TokenType.Whitespaces) };
            tokensList.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SplitToTokens_ShouldReturnCorrectTokensList_IfEscapedSymbolsInText()
        {
            var text = @"hello\_world";

            var tokensList = tokenReader.SplitToTokens(text);

            var expected = new List<Token>
            {
                new Token(0, "hello", TokenType.Letters),
                new Token(5, @"\_", TokenType.EscapedSymbol),
                new Token(7, "world", TokenType.Letters)
            };
            tokensList.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SplitToTokens_ShouldReturnCorrectTokensList_IfEscapedSymbolsInTextStart()
        {
            var text = @"\\hello";

            var tokensList = tokenReader.SplitToTokens(text);

            var expected = new List<Token>
            {
                new Token(0, @"\\", TokenType.EscapedSymbol),
                new Token(2, "hello", TokenType.Letters)
            };
            tokensList.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SplitToTokens_ShouldReturnCorrectTokensList_IfEscapedSymbolsInTextEnd()
        {
            var text = @"hello\a";

            var tokensList = tokenReader.SplitToTokens(text);

            var expected = new List<Token>
            {
                new Token(0, "hello", TokenType.Letters),
                new Token(5, @"\a", TokenType.EscapedSymbol)
            };
            tokensList.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SplitToTokens_ShouldReturnCorrectTokensList_IfSeveralTokenTypes()
        {
            var text = "abc   123";

            var tokensList = tokenReader.SplitToTokens(text);

            var expected = new List<Token>
            {
                new Token(0, "abc", TokenType.Letters),
                new Token(3, "   ", TokenType.Whitespaces),
                new Token(6, "123", TokenType.Number)
            };
            tokensList.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SplitToTokens_ShouldReturnCorrectTokensList_IfTextSurroundedByUnderscores()
        {
            var text = "_abc_";

            var tokensList = tokenReader.SplitToTokens(text);

            var expected = new List<Token>
            {
                new Token(0, "_", TokenType.Underscore),
                new Token(1, "abc", TokenType.Letters),
                new Token(4, "_", TokenType.Underscore)
            };
            tokensList.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SplitToTokens_ShouldReturnCorrectTokensList_IfTextSurroundedByDoubleUnderscores()
        {
            var text = "__abc__";

            var tokensList = tokenReader.SplitToTokens(text);

            var expected = new List<Token>
            {
                new Token(0, "_", TokenType.Underscore),
                new Token(1, "_", TokenType.Underscore),
                new Token(2, "abc", TokenType.Letters),
                new Token(5, "_", TokenType.Underscore),
                new Token(6, "_", TokenType.Underscore)
            };
            tokensList.ShouldBeEquivalentTo(expected);
        }
    }
}
