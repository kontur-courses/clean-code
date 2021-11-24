using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.SyntaxParser;
using Markdown.Tokens;
using Markdown.Tokens.ConcreteTokens;
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

        [Test]
        public void Should_Parse_ImageCorrectly()
        {
            var lexedTokens = new[]
                {Token.OpenImageAlt, Token.Text("abc"), Token.CloseImageAlt, Token.Text("a"), Token.CloseImageTag};
            var expected = new TokenTree[]
            {
                new(new ImageToken("a", "abc"))
            };
            var parsed = sut.Parse(lexedTokens);
            parsed.Should().Equal(expected);
        }
        
        [Test]
        public void Should_Parse_ImageWithItalicsCorrectly()
        {
            var lexedTokens = new[]
                {
                    Token.Italics, Token.OpenImageAlt, Token.Text("abc"), 
                    Token.CloseImageAlt, Token.Text("a"), Token.CloseImageTag, 
                    Token.Italics
                };
            var expected = new TokenTree[]
            {
                new(Token.Italics,
                    new TokenTree(new ImageToken("a", "abc")))
            };
            var parsed = sut.Parse(lexedTokens);
            parsed.Should().Equal(expected);
        }
        
        [Test]
        public void Should_Parse_Image_WhenTextAfter()
        {
            var lexedTokens = new[]
            {
                Token.OpenImageAlt, Token.Text("abc"), 
                Token.CloseImageAlt, Token.Text("a"), Token.CloseImageTag, Token.Text("aaa")
            };
            var expected = new TokenTree[]
            {
                new(new ImageToken("a", "abc")),
                TokenTree.FromText("aaa")
            };
            var parsed = sut.Parse(lexedTokens);
            parsed.Should().Equal(expected);
        }
    }
}