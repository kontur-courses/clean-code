using FluentAssertions;
using Markdown.TokenizerClasses;
using Markdown.TokenizerClasses.Scanners;
using NUnit.Framework;

namespace Markdown.Tests.TokenizerClassesTests
{
    [TestFixture]
    public class PlainTextScanner_Should
    {
        private readonly PlainTextScanner scanner = new PlainTextScanner();

        [TestCase("cheap\n", "cheap", TestName = "before newline")]
        [TestCase("flesh\r", "flesh", TestName = "before carriage return")]
        [TestCase("tap ", "tap", TestName = "before space")]
        [TestCase("mercy\\", "mercy", TestName = "before escape character")]
        [TestCase("develop_", "develop", TestName = "before underscore")]
        [TestCase("organize8", "organize", TestName = "before digit")]
        [TestCase("bind", "bind", TestName = "only plain text")]
        public void TryScan_ValidText_SuccessfullyAndReturnAppropriateToken(string text, string expectedValue)
        {
            var expected = new Token(TokenType.Text, expectedValue);

            scanner.TryScan(text, out var token)
                .Should()
                .BeTrue();

            token.Should()
                .BeEquivalentTo(expected);
        }

        [TestCase("", TestName = "empty string")]
        [TestCase(null, TestName = "null")]
        [TestCase("\n", TestName = "newline")]
        [TestCase("\r", TestName = "carriage return")]
        [TestCase(" ", TestName = "space")]
        [TestCase("\\", TestName = "escape char")]
        [TestCase("_", TestName = "underscore tag")]
        [TestCase("7", TestName = "digit")]
        public void TryScan_InvalidText_UnsuccessfullyAndReturnNullToken(string text)
        {
            scanner.TryScan(text, out var token)
                .Should()
                .BeFalse();

            token.Should()
                .BeEquivalentTo(Token.Null);
        }
    }
}
