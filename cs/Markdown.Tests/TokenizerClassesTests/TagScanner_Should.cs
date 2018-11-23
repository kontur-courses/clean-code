using FluentAssertions;
using Markdown.TokenizerClasses;
using Markdown.TokenizerClasses.Scanners;
using NUnit.Framework;

namespace Markdown.Tests.TokenizerClassesTests
{
    [TestFixture]
    public class TagScanner_Should
    {
        private readonly TagScanner scanner = new TagScanner();

        [TestCase("_", TokenType.Underscore, TestName = "underscore")]
        [TestCase("\\", TokenType.EscapeChar, TestName = "escape character")]
        [TestCase(" ", TokenType.Space, TestName = "space")]
        [TestCase("\r", TokenType.CarriageReturn, TestName = "carriage return")]
        [TestCase("\n", TokenType.Newline, TestName = "newline")]
        public void TryScan_ValidTag_SuccessfullyAndReturnAppropriateToken(string tag, TokenType expectedType)
        {
            var expected = new Token(expectedType, tag);

            scanner.TryScan(tag, out var token)
                   .Should()
                   .BeTrue();

            token.Should()
                 .BeEquivalentTo(expected);
        }

        [TestCase("0", TestName = "digit 0")]
        [TestCase("1", TestName = "digit 1")]
        [TestCase("2", TestName = "digit 2")]
        [TestCase("3", TestName = "digit 3")]
        [TestCase("4", TestName = "digit 4")]
        [TestCase("5", TestName = "digit 5")]
        [TestCase("6", TestName = "digit 6")]
        [TestCase("7", TestName = "digit 7")]
        [TestCase("8", TestName = "digit 8")]
        [TestCase("9", TestName = "digit 9")]
        public void TryScan_Num_SuccessfullyAndReturnAppropriateToken(string digit)
        {
            var expected = new Token(TokenType.Num, digit);

            scanner.TryScan(digit, out var token)
                .Should()
                .BeTrue();

            token.Should()
                .BeEquivalentTo(expected);
        }

        [TestCase(null, TestName = "null")]
        [TestCase("", TestName = "empty")]
        public void TryScan_NullTag_UnsuccessfullyAndReturnNullToken(string tag)
        {
            scanner.TryScan(tag, out var token)
                .Should()
                .BeFalse();

            token.Should()
                .BeEquivalentTo(Token.Null);
        }

        [TestCase(".", TestName = "dot")]
        [TestCase("(", TestName = "open bracket")]
        [TestCase("a", TestName = "alphabetical")]
        public void TryScan_InvalidTag_UnsuccessfullyAndReturnNullToken(string tag)
        {
            scanner.TryScan(tag, out var token)
                .Should()
                .BeFalse();

            token.Should()
                .BeEquivalentTo(Token.Null);
        }
    }
}