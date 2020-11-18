using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class HTMLConverter_Should
    {
        private HTMLConverter htmlConverter;

        [SetUp]
        public void SetUp()
        {
            var tokensText = new Dictionary<TokenType, string>
            {
                {TokenType.Text, ""},
                {TokenType.Emphasized, "em"},
                {TokenType.Header, "h1"},
                {TokenType.Strong, "strong"}
            };
            htmlConverter = new HTMLConverter(tokensText);
        }

        [Test]
        public void GetHTMLString_CorrectStringLine_OnlyTextTokens()
        {
            var textTokens = new List<TextToken>
            {
                new TextToken(2, TokenType.Text, "aa", true),
                new TextToken(4, TokenType.Text, "cccc", true)
            };
            var expectedString = "aacccc";

            var actualString = htmlConverter.ConvertTokens(textTokens);

            actualString.Should().BeEquivalentTo(expectedString);
        }

        [Test]
        public void GetHTMLString_CorrectStringLine_TextTokensAndEmTokens()
        {
            var textTokens = new List<TextToken>
            {
                new TextToken(2, TokenType.Text, "aa", true),
                new TextToken(4, TokenType.Emphasized, "cccc",
                    new List<TextToken>
                    {
                        new TextToken(4, TokenType.Text, "cccc", true)
                    }),
                new TextToken(2, TokenType.Text, "bb", true),
                new TextToken(4, TokenType.Emphasized, "dddd",
                    new List<TextToken>
                    {
                        new TextToken(4, TokenType.Text, "dddd", true)
                    })
            };
            var expectedString = "aa<em>cccc</em>bb<em>dddd</em>";

            var actualString = htmlConverter.ConvertTokens(textTokens);

            actualString.Should().BeEquivalentTo(expectedString);
        }

        [Test]
        public void GetHTMLString_CorrectStringLine_EmTokenInsideStrong()
        {
            var textTokens = new List<TextToken>
            {
                new TextToken(2, TokenType.Strong, "aa", new List<TextToken>
                {
                    new TextToken(4, TokenType.Text, "cccc", true),
                    new TextToken(2, TokenType.Emphasized, "ab", new List<TextToken>
                    {
                        new TextToken(2, TokenType.Text, "ab", true)
                    }),
                    new TextToken(4, TokenType.Text, "bbbb", true)
                })
            };
            var expectedString = "<strong>cccc<em>ab</em>bbbb</strong>";

            var actualString = htmlConverter.ConvertTokens(textTokens);

            actualString.Should().BeEquivalentTo(expectedString);
        }

        [Test]
        public void GetHTMLString_CorrectStringLine_HeaderToken()
        {
            var textTokens = new List<TextToken>
            {
                new TextToken(2, TokenType.Header, "aa", new List<TextToken>
                {
                    new TextToken(4, TokenType.Text, "cccc", true),
                    new TextToken(2, TokenType.Emphasized, "ab", new List<TextToken>
                    {
                        new TextToken(2, TokenType.Text, "ab", true)
                    }),
                    new TextToken(4, TokenType.Text, "bbbb", true)
                })
            };
            var expectedString = "<h1>cccc<em>ab</em>bbbb</h1>";

            var actualString = htmlConverter.ConvertTokens(textTokens);

            actualString.Should().BeEquivalentTo(expectedString);
        }
    }
}