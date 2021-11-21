using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.SyntaxParser;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests.SyntaxParser
{
    public class SyntaxParserTests
    {
        private MarkdownSyntaxParser sut;

        [SetUp]
        public void InitParser()
        {
            sut = new MarkdownSyntaxParser();
        }

        [Test]
        public void Parser_Should_ThrowException_WhenInputIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.Parse(null).ToArray());
        }

        [TestCaseSource(typeof(SyntaxParserTestData), nameof(SyntaxParserTestData.ShouldNotBeApplied))]
        public void Parser_Should_NotBeApplied_When(IEnumerable<Token> lexed, IEnumerable<TokenTree> expected)
        {
            sut.Parse(lexed).Should().Equal(expected);
        }

        [TestCaseSource(typeof(SyntaxParserTestData), nameof(SyntaxParserTestData.ItalicsData))]
        [TestCaseSource(typeof(SyntaxParserTestData), nameof(SyntaxParserTestData.BoldData))]
        [TestCaseSource(typeof(SyntaxParserTestData), nameof(SyntaxParserTestData.Header1Data))]
        [TestCaseSource(typeof(SyntaxParserTestData), nameof(SyntaxParserTestData.EscapeSymbolData))]
        public void Parser_Should_ParseCorrectly_When(IEnumerable<Token> lexed, IEnumerable<TokenTree> expected)
        {
            var parsedTokens = sut.Parse(lexed);

            parsedTokens.Should().Equal(expected);
        }
    }
}