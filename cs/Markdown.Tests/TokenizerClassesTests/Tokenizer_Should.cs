using System.Collections.Generic;
using FluentAssertions;
using Markdown.TokenizerClasses;
using NUnit.Framework;

namespace Markdown.Tests.TokenizerClassesTests
{
    [TestFixture]
    public class Tokenizer_Should
    {
        private readonly Tokenizer tokenizer = new Tokenizer();

        [Test]
        public void Tokenize_TextsAndSpaces_Successfully()
        {
            var text = "Cup of Joe";
            var expected = new List<Token>
            {
                new Token(TokenType.Text, "Cup"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "of"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "Joe")
            };

            tokenizer.Tokenize(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_Nums_Successfully()
        {
            var text = "0If 1You 2Can't 3Stand 4the 5Heat, 6Get 7Out 8the 9Kitchen";
            var expected = new List<Token>
            {
                new Token(TokenType.Num, "0"),
                new Token(TokenType.Text, "If"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Num, "1"),
                new Token(TokenType.Text, "You"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Num, "2"),
                new Token(TokenType.Text, "Can't"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Num, "3"),
                new Token(TokenType.Text, "Stand"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Num, "4"),
                new Token(TokenType.Text, "the"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Num, "5"),
                new Token(TokenType.Text, "Heat,"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Num, "6"),
                new Token(TokenType.Text, "Get"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Num, "7"),
                new Token(TokenType.Text, "Out"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Num, "8"),
                new Token(TokenType.Text, "the"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Num, "9"),
                new Token(TokenType.Text, "Kitchen"),
            };

            tokenizer.Tokenize(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_Underscore_Successfully()
        {
            var text = "Ugly 0_";
            var expected = new List<Token>
            {
                new Token(TokenType.Text, "Ugly"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Num, "0"),
                new Token(TokenType.Underscore, "_"),
            };

            tokenizer.Tokenize(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_DoubleUnderscore_Successfully()
        {
            var text = @"express 7__";
            var expected = new List<Token>
            {
                new Token(TokenType.Text, "express"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Num, "7"),
                new Token(TokenType.DoubleUnderscore, "__")
            };

            tokenizer.Tokenize(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [TestCase(@"\_", @"_", TestName = "underscore")]
        [TestCase(@"\\", @"\", TestName = "escape char")]
        public void Tokenize_EscapedTokenAsTextToken(string escapedToken, string expectedValue)
        {
            var expected = new Token(TokenType.Text, expectedValue);

            tokenizer.Tokenize(escapedToken)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_EscapedDoubleUnderscoreAsTwoTextUnderscores()
        {
            var text = @"\_\_";
            var expected = new List<Token>
            {
                new Token(TokenType.Text, "_"),
                new Token(TokenType.Text, "_")
            };

            tokenizer.Tokenize(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_EscapeChar_Successfully()
        {
            var text = @"\ _0 satisfaction";
            var expected = new List<Token>
            {
                new Token(TokenType.EscapeChar, @"\"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Underscore, "_"),
                new Token(TokenType.Num, "0"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "satisfaction")
            };

            tokenizer.Tokenize(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [TestCase("73", TestName = "two-digit num")]
        [TestCase("314", TestName = "three-digit num")]
        [TestCase("1234567890", TestName = "ten-digit num")]
        public void Tokenize_ConsecutiveNumsAsSingleNum(string text)
        {
            var expected = new Token(TokenType.Num, text);

            tokenizer.Tokenize(text)
                .Should()
                .BeEquivalentTo(expected);

        }

        [TestCase("", TestName = "empty text")]
        [TestCase(null, TestName = "null")]
        public void GetNextToken_OnNullOrEmptyText_ShouldReturnEOFToken(string text)
        {
            tokenizer.GetNextToken(text)
                .Should()
                .BeEquivalentTo(Token.EOF);
        }

        [TestCase("0", TokenType.Num, "0", TestName = "digit 0")]
        [TestCase("1", TokenType.Num, "1", TestName = "digit 1")]
        [TestCase("2", TokenType.Num, "2", TestName = "digit 2")]
        [TestCase("3", TokenType.Num, "3", TestName = "digit 3")]
        [TestCase("4", TokenType.Num, "4", TestName = "digit 4")]
        [TestCase("5", TokenType.Num, "5", TestName = "digit 5")]
        [TestCase("6", TokenType.Num, "6", TestName = "digit 6")]
        [TestCase("7", TokenType.Num, "7", TestName = "digit 7")]
        [TestCase("8", TokenType.Num, "8", TestName = "digit 8")]
        [TestCase("9", TokenType.Num, "9", TestName = "digit 9")]
        [TestCase("_", TokenType.Underscore, "_", TestName = "underscore tag")]
        [TestCase("\\", TokenType.EscapeChar, "\\", TestName = "escape character")]
        [TestCase(" ", TokenType.Space, " ", TestName = "space")]
        [TestCase("\r", TokenType.CarriageReturn, "\r", TestName = "carriage return")]
        [TestCase("\n", TokenType.Newline, "\n", TestName = "newline")]
        [TestCase("difficult\n", TokenType.Text, "difficult", TestName = "text before newline")]
        [TestCase("artificial\r", TokenType.Text, "artificial", TestName = "text before carriage return")]
        [TestCase("passage ", TokenType.Text, "passage", TestName = "text before space")]
        [TestCase("reality\\", TokenType.Text, "reality", TestName = "text before escape character")]
        [TestCase("reinforce_", TokenType.Text, "reinforce", TestName = "text before underscore")]
        [TestCase("bloody5", TokenType.Text, "bloody", TestName = "text before digit")]
        [TestCase("penalty", TokenType.Text, "penalty", TestName = "only plain text")]
        public void GetNextToken_FromValidText_ReturnAppropriateToken(string text, TokenType expectedType, string expectedValue)
        {
            var expected = new Token(expectedType, expectedValue);

            tokenizer.GetNextToken(text)
                .Should()
                .BeEquivalentTo(expected);
        }
    }
}