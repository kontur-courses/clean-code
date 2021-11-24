using System;
using System.Linq;
using FluentAssertions;
using Markdown.Lexer;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests.Lexer
{
    public class LexerTests
    {
        private MarkdownLexer sut;

        [SetUp]
        public void InitLexer()
        {
            sut = new MarkdownLexer();
        }

        [Test]
        public void Lex_Should_ThrowException_WhenTextIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.Lex(null).ToArray());
        }

        [TestCase("")]
        [TestCase("text")]
        [TestCase("text with spaces")]
        public void Lex_Should_Return_TextToken_WhenTextWithoutFormatting(string text)
        {
            var tokens = sut.Lex(text).ToList();

            tokens.Should().HaveCount(1);
            tokens.First().TokenType.Should().Be(TokenType.Text);
        }

        [TestCaseSource(typeof(LexerTestDataGenerator))]
        public void Lex_Should_Return_CorrectTokens_When(string text, Token[] expectedResult)
        {
            var tokens = sut.Lex(text);

            tokens.Should().Equal(expectedResult);
        }

        [Test]
        public void Lex_Should_Lex_Image()
        {
            var text = "![alt](src)";
            var expected = new[]
            {
                Token.OpenImageAlt, Token.Text("alt"), Token.CloseImageAlt,
                Token.Text("src"), Token.CloseImageTag
            };

            var tokens = sut.Lex(text);

            tokens.Should().Equal(expected);
        }
    }
}