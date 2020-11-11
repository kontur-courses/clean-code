using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FluentAssertions;
using Markdown.Parsers;
using NUnit.Framework;

namespace Markdown.Tests.ParserTests
{
    [TestFixture]
    public class BoldParserTests
    {
        private TokenReader reader;

        [SetUp]
        public void SetUp()
        {
            reader = new TokenReader();
        }

        [Test]
        public void SimpleLine()
        {
            var token = reader.ReadTokens("__abc__").First();
            var expected = new Token(0, "abc", TokenType.Bold);
            token.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ItalicTokenIntoBold()
        {
            var token = reader.ReadTokens("__abc_cd_ef__").First();
            var expected = new Token(0, "abcef", TokenType.Bold);
            expected.SetNestedTokens(new List<Token>() { new Token(3, "cd", TokenType.Italic) });
            token.Should().BeEquivalentTo(expected);
        }
    }
}
