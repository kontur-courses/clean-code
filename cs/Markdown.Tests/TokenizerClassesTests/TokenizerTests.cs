using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown.TokenizerClasses;
using NUnit.Framework;

namespace Markdown.Tests.TokenizerClassesTests
{
    [TestFixture]
    class TokenizerTests
    {
        private readonly Tokenizer tokenizer = new Tokenizer();

        [Test]
        public void TokenizeToList_OneUnderscoreText_ShouldTokenize()
        {
            var text = "_text_";
            var tokens = new List<Token>
            {
                new Token("UNDERSCORE", "_"),
                new Token("TEXT", "text"),
                new Token("UNDERSCORE", "_")
            };

            tokenizer.TokenizeToList(text).Should().BeEquivalentTo(tokens);
        }

        [Test]
        public void TokenizeToList_EscapedUnderscore__ShouldTokenize()
        {
            var text = "_\\text\\_";
            var tokens = new List<Token>
            {
                new Token("UNDERSCORE", "_"),
                new Token("ESCAPE", "\\"),
                new Token("TEXT", "text"),
                new Token("UNDERSCORE", "_"),
                new Token("ESCAPE", "\\"),
            };

            tokenizer.TokenizeToList(text).Should().BeEquivalentTo(tokens);
        }

        [Test]
        public void TokenizeToList_FinishWithNewline_ShouldTokenize()
        {
            var text = "_\\text\n";
            var tokens = new List<Token>
            {
                new Token("UNDERSCORE", "_"),
                new Token("ESCAPE", "\\"),
                new Token("TEXT", "text"),
                new Token("NEWLINE", "\n")
            };

            tokenizer.TokenizeToList(text).Should().BeEquivalentTo(tokens);
        }

        [TestCase(null, TestName = "null")]
        [TestCase("", TestName = "empty string")]
        public void TokenizeToList_InvalidText_ReturnEOF(string text)
        {
            var expected = new List<Token>
            {
                new Token("EOF", "")
            };

            tokenizer.TokenizeToList(text).Should().BeEquivalentTo(expected);
        }

        [TestCase("hello_", "TEXT", "hello", TestName = "plain text")]
        [TestCase("_what is that", "UNDERSCORE", "_", TestName = "underscore")]
        [TestCase("\\bye bye", "ESCAPE", "\\", TestName = "escape")]
        [TestCase("\nnewline", "NEWLINE", "\n", TestName = "newline")]
        public void ScanToken_ValidText_ShouldScan(string text, string tokenType, string tokenValue)
        {
            tokenizer.ScanToken(text).Should().BeEquivalentTo(new Token(tokenType, tokenValue));
        }

        [TestCase(null, TestName = "null")]
        [TestCase("", TestName = "empty string")]
        public void ScanToken_InvalidText_ReturnNull(string text)
        {
            tokenizer.ScanToken(text).Should().BeNull();
        }
    }
}
