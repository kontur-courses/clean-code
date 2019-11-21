using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    class TokenReaderTests
    {
        private readonly TokenReader tokenReader = new TokenReader(Md.MdTokenDescriptions);

        [Test]
        public void SplitToTokens_ShouldReturnCorrectTokensList_IfOnlyLetters()
        {
            var text = "abc";

            var tokensList = tokenReader.SplitToTokens(text);

            var expected = new List<Token> {new Token(TokenType.Letters, 0, "abc")};
            tokensList.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SplitToTokens_ShouldReturnCorrectTokensList_IfOnlyNumber()
        {
            var text = "123";

            var tokensList = tokenReader.SplitToTokens(text);

            var expected = new List<Token> { new Token(TokenType.Number, 0, "123") };
            tokensList.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SplitToTokens_ShouldReturnCorrectTokensList_IfOnlyWhiteSpaces()
        {
            var text = "   ";

            var tokensList = tokenReader.SplitToTokens(text);

            var expected = new List<Token> { new Token(TokenType.Whitespaces, 0, "   ") };
            tokensList.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SplitToTokens_ShouldReturnCorrectTokensList_IfEscapedSymbolsInText()
        {
            var text = @"hello\_world";

            var tokensList = tokenReader.SplitToTokens(text);

            var expected = new List<Token>
            {
                new Token(TokenType.Letters, 0, "hello"),
                new Token(TokenType.EscapedSymbol, 5, @"\_"),
                new Token(TokenType.Letters, 7, "world")
            };
            tokensList.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SplitToTokens_ShouldReturnCorrectTokensList_IfEscapeSymbolInTextEnd()
        {
            var text = @"hello\";

            var tokensList = tokenReader.SplitToTokens(text);

            var expected = new List<Token>
            {
                new Token(TokenType.Letters, 0, @"hello"),
                new Token(TokenType.Symbols, 5, @"\")
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
                new Token(TokenType.Letters, 0, "abc"),
                new Token(TokenType.Whitespaces, 3, "   "),
                new Token(TokenType.Number, 6, "123")
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
                new Token(TokenType.Underscore, 0),
                new Token(TokenType.Letters, 1, "abc"),
                new Token(TokenType.Underscore, 4)
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
                new Token(TokenType.DoubleUnderscores, 0),
                new Token(TokenType.Letters, 2, "abc"),
                new Token(TokenType.DoubleUnderscores, 5),
            };
            tokensList.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SplitToTokens_ShouldReturnCorrectTokensList_IfTextSurroundedByTripleUnderscores()
        {
            var text = "___abc___";

            var tokensList = tokenReader.SplitToTokens(text);

            var expected = new List<Token>
            {
                new Token(TokenType.DoubleUnderscores, 0),
                new Token(TokenType.Underscore, 2),
                new Token(TokenType.Letters, 3, "abc"),
                new Token(TokenType.DoubleUnderscores, 6),
                new Token(TokenType.Underscore, 8)
            };
            tokensList.ShouldBeEquivalentTo(expected);
        }
    }
}
