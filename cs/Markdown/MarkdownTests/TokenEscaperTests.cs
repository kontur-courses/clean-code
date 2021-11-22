using System;
using FluentAssertions;
using Markdown.Models;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class TokenEscaperTests
    {
        [Test]
        public void Constructor_ThrowsException_TextNull()
        {
            Assert.Throws<ArgumentException>(() =>
                new TokenEscaper(null, new[] {MarkdownTokensFactory.Italic()}));
        }

        [Test]
        public void Constructor_ThrowsException_TokensNull()
        {
            Assert.Throws<ArgumentException>(() =>
                new TokenEscaper("qwe", null));
        }

        [Test]
        public void EscapeTokens_RemovesOnlyEscapedTokens()
        {
            new TokenEscaper(@"\__a_", new[] {MarkdownTokensFactory.Italic()})
                .EscapeTokens()
                .Text.Should().Be("_a_");
        }

        [Test]
        public void IsEscapeSymbol_True()
        {
            new TokenEscaper(@"\__a_", new[] {MarkdownTokensFactory.Italic()})
                .IsEscapeSymbol(0)
                .Should().BeTrue();
        }

        [Test]
        public void IsEscapeSymbol_False()
        {
            new TokenEscaper(@"\__a_", new[] {MarkdownTokensFactory.Italic()})
                .IsEscapeSymbol(1)
                .Should().BeFalse();
        }
    }
}