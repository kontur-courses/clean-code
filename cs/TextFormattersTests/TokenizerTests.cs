using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown;
using NUnit.Framework;

namespace TextFormattersTests
{
    [TestFixture]
    public class TokenizerTests
    {
        [TestCase("a b", TokenType.Word, TokenType.Space, TokenType.Word, TestName = "Simple line")]
        [TestCase("1a_", TokenType.Number, TokenType.Word, TokenType.Tag, TestName = "More than one tag in word")]
        [TestCase(@"\_ab", TokenType.SymbolSet, TokenType.Word, TestName = "Shield symbol")]
        [TestCase("", TestName = "Empty input => empty tokens")]
        [TestCase("a\nb", TokenType.Word, TokenType.BreakLine, TokenType.Word, TestName = "Break line is token too")]
        [TestCase("&", TokenType.SymbolSet, TestName = "Symbol which isn't tag is symbol")]
        public void TypeTests(string input, params TokenType[] expected)
        {
            var tokenizer = new Tokenizer(new Md(), input);

            var tokens = tokenizer.ToArray();
            Assert.AreEqual(expected.Length, tokens.Length);
            Assert.AreEqual(expected, tokens.Select(x => x.Type));
        }
    }
}
