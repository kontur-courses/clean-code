using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.Tests.ParserTests
{
    [TestFixture]
    public class HeaderParserTests
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
            var token = reader.ReadTokens("#abcd").First();
            var expected = new Token(0, "abcd", TokenType.Header);

            token.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ItalicTokenIntoHeader()
        {
            var token = reader.ReadTokens("#ab_c_d").First();
            var expected = new Token(0, "abd", TokenType.Header);
            expected.SetNestedTokens(new List<Token>() { new Token(2, "c", TokenType.Italic) });

            token.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void BoldTokenIntoHeader()
        {
            var token = reader.ReadTokens("#ab__c__d").First();
            var expected = new Token(0, "abd", TokenType.Header);
            expected.SetNestedTokens(new List<Token>() { new Token(2, "c", TokenType.Bold) });

            token.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ItalicInBoldTokenIntoHeader()
        {
            var token = reader.ReadTokens("#a__b_c_d__ef").First();
            var expected = new Token(0, "aef", TokenType.Header);
            var boldToken = new Token(1, "bd", TokenType.Bold);
            boldToken.SetNestedTokens(new List<Token>() { new Token(1, "c", TokenType.Italic) });
            expected.SetNestedTokens(new List<Token>() { boldToken});
            token.Should().BeEquivalentTo(expected);
        }
    }
}
