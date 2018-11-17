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
        public void Tokenize_TextsSpacesAndDigits_Successfully()
        {
            var text = "0If 1You 2Can't 3Stand 4the 5Heat, 6Get 7Out 8the 9Kitchen";
            var expected = new List<Token>
            {
                new Token(TokenType.Digit, "0"),
                new Token(TokenType.Text, "If"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Digit, "1"),
                new Token(TokenType.Text, "You"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Digit, "2"),
                new Token(TokenType.Text, "Can't"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Digit, "3"),
                new Token(TokenType.Text, "Stand"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Digit, "4"),
                new Token(TokenType.Text, "the"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Digit, "5"),
                new Token(TokenType.Text, "Heat,"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Digit, "6"),
                new Token(TokenType.Text, "Get"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Digit, "7"),
                new Token(TokenType.Text, "Out"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Digit, "8"),
                new Token(TokenType.Text, "the"),
                new Token(TokenType.Space, " "),

                new Token(TokenType.Digit, "9"),
                new Token(TokenType.Text, "Kitchen"),
            };

            tokenizer.Tokenize(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_TextsSpacesDigitsAndUnderscores_Successfully()
        {
            var text = "_0Ugly _1Duckling";
            var expected = new List<Token>
            {
                new Token(TokenType.Underscore, "_"),
                new Token(TokenType.Digit, "0"),
                new Token(TokenType.Text, "Ugly"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Underscore, "_"),
                new Token(TokenType.Digit, "1"),
                new Token(TokenType.Text, "Duckling"),
            };

            tokenizer.Tokenize(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_TextsSpacesDigitsUnderscoresAndEscapeChars_Successfully()
        {
            var text = "\\_0 satisfaction";
            var expected = new List<Token>
            {
                new Token(TokenType.EscapeChar, "\\"),
                new Token(TokenType.Underscore, "_"),
                new Token(TokenType.Digit, "0"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "satisfaction")
            };

            tokenizer.Tokenize(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_TextsSpacesDigitsUnderscoresEscapeCharsAndCarriageReturn_Successfully()
        {
            var text = " 1\\_cherry\r";
            var expected = new List<Token>
            {
                new Token(TokenType.Space, " "),
                new Token(TokenType.Digit, "1"),
                new Token(TokenType.EscapeChar, "\\"),
                new Token(TokenType.Underscore, "_"),
                new Token(TokenType.Text, "cherry"),
                new Token(TokenType.CarriageReturn, "\r")
            };

            tokenizer.Tokenize(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_TextsSpacesDigitsUnderscoresEscapeCharsCarriageReturnsAndNewlines_Successfully()
        {
            var text = "0\\_Playing Possum\r\n";
            var expected = new List<Token>
            {
                new Token(TokenType.Digit, "0"),
                new Token(TokenType.EscapeChar, "\\"),
                new Token(TokenType.Underscore, "_"),
                new Token(TokenType.Text, "Playing"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "Possum"),
                new Token(TokenType.CarriageReturn, "\r"),
                new Token(TokenType.Newline, "\n")
            };

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

        [TestCase("0", TokenType.Digit, "0", TestName = "digit 0")]
        [TestCase("1", TokenType.Digit, "1", TestName = "digit 1")]
        [TestCase("2", TokenType.Digit, "2", TestName = "digit 2")]
        [TestCase("3", TokenType.Digit, "3", TestName = "digit 3")]
        [TestCase("4", TokenType.Digit, "4", TestName = "digit 4")]
        [TestCase("5", TokenType.Digit, "5", TestName = "digit 5")]
        [TestCase("6", TokenType.Digit, "6", TestName = "digit 6")]
        [TestCase("7", TokenType.Digit, "7", TestName = "digit 7")]
        [TestCase("8", TokenType.Digit, "8", TestName = "digit 8")]
        [TestCase("9", TokenType.Digit, "9", TestName = "digit 9")]
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