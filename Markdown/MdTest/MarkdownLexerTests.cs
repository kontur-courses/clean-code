using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MdTest
{
    [TestFixture]
    public class MarkdownLexerTests
    {
        [Test]
        public void Tokenize_ShouldReturnCorrectTokens()
        {
            var testingString = "[blabla]()";
            var expectedResult = new[]
            {
                new Token("[", TokenType.OpeningLinkNameDelimiter),
                new Token("blabla", TokenType.SimpleWord),
                new Token("]", TokenType.ClosingLinkNameDelimiter),
                new Token("(", TokenType.OpeningLinkHrefDelimiter),
                new Token(")", TokenType.ClosingLinkHrefDelimiter),
            };
            var lexer = new MarkdownLexer(testingString);
            var actual = lexer.Tokenize();
            actual.Should().BeEquivalentTo(expectedResult);
        }
    }
}
