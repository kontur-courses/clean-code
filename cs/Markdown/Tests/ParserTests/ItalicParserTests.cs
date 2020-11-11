using FluentAssertions;
using Markdown.Parsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.Tests.ParserTests
{
    [TestFixture]
    public class ItalicParserTests
    {
        private TokenReader reader;

        [SetUp]
        public void SetUp()
        {
            reader = new TokenReader();
        }

        [Test]
        public void SimpleLineTest()
        {
            var token = reader.ReadTokens("_abcd_").First();
            var expected = new Token(0, "abcd", TokenType.Italic);
            token.Should().BeEquivalentTo(expected);
        }
    }
}
