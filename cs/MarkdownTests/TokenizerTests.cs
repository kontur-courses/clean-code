using Markdown;
using NUnit.Framework;
using System;
using System.Linq;
using FluentAssertions;

namespace MarkdownTests
{
    [TestFixture]
    public class TokenizerTests
    {
        [TestCase("", TestName = "empty string")]
        [TestCase("aaaaa   bbbbb   1111 ^^^", TokenType.Word, TokenType.Space, TokenType.Word,
            TokenType.Space, TokenType.Number, TokenType.Space, TokenType.OtherSymbol, TestName = "token types without tag")]
        [TestCase("1a_", TokenType.Number, TokenType.Word, TokenType.Tag, TestName = "token types with tag")]
        [TestCase(@"\_", TokenType.OtherSymbol, TestName = "shield tag")]
        [TestCase(@"\_ab", TokenType.OtherSymbol, TokenType.Word, TestName = "shield tag in word")]
        [TestCase("a\nb", TokenType.Word, TokenType.BreakLine, TokenType.Word, TestName = "breakline is token")]
        [TestCase("a\\nb", TokenType.Word, TokenType.OtherSymbol, TokenType.Word, TestName = "shield breakline")]
        [TestCase("Здесь сим\\волы", TokenType.Word, TokenType.Space, TokenType.Word, TokenType.OtherSymbol,
            TokenType.Word, TestName = "don't shield not special symbols")]
        [TestCase(@"\\_", TokenType.OtherSymbol, TokenType.Tag, TestName = "shield shield symbol")]

        public void ParseToTokensTest(string input, params TokenType[] expected)
        {
            var tokens = new Tokenizer(new Md(), input).ParseToTokens().tokens.Select(t => t.Type).ToArray();
            tokens.Should().HaveCount(expected.Length).And.BeEquivalentTo(expected);
        }
    }
}
