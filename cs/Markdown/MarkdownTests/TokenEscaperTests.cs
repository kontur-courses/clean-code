using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class TokenEscaperTests
    {
        private TokenEscaper escaper;

        [SetUp]
        public void SetUp()
        {
            escaper = new TokenEscaper(new List<IToken> {new ItalicToken()});
        }

        [Test]
        public void Constructor_ThrowsException_IfTokensNull() =>
            Assert.That(() => new TokenEscaper(null), Throws.InstanceOf<ArgumentException>());

        [Test]
        public void EscapeTokens_ThrowsException_IfTextNull() =>
            Assert.That(() => escaper.EscapeTokens(null), Throws.InstanceOf<ArgumentException>());

        [Test]
        public void EscapeTokens_RemovesOnlyEscapedTokens()
        {
            escaper
                .EscapeTokens(@"\__a_")
                .Text.Should().Be("_a_");
        }

        [Test]
        public void IsEscapeSymbol_True_IfNextSymbolTagStart()
        {
            escaper
                .IsEscapeSymbol(new Context(@"\__a_"))
                .Should().BeTrue();
        }

        [Test]
        public void IsEscapeSymbol_False_IfSymbolNotSlash()
        {
            escaper
                .IsEscapeSymbol(new Context(@"\__a_", 1))
                .Should().BeFalse();
        }

        [TestCase("a")]
        [TestCase("1")]
        [TestCase(" ")]
        [TestCase("\t")]
        [TestCase("\n")]
        [TestCase("\r")]
        public void IsEscapeSymbol_False_IfNextSymbol(string nextSymbol)
        {
            escaper
                .IsEscapeSymbol(new Context($"\\{nextSymbol}_a_"))
                .Should().BeFalse();
        }
    }
}