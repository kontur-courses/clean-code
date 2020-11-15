using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using Markdown.TokenConverters;

namespace Markdown.Tests
{
    public class HTMLConverter_Should
    {
        private List<ITokenConverter> tokenConverters;

        [SetUp]
        public void SetUp()
        {
            tokenConverters = new List<ITokenConverter>
            {
                new StrongTokenConverter(),
                new EmphasizedTokenConverter(),
                new TextTokenConverter()
            };
        }
        
        [Test]
        public void GetHTMLString_CorrectStringLine_OnlyTextTokens()
        {
            var textTokens = new List<TextToken>
            {
                new TextToken(0,2,TokenType.Text, "aa"),
                new TextToken(2,4,TokenType.Text,"cccc")
            };
            var htmlConverter = new HTMLConverter();
            var expectedString = "aacccc";
            
            var actualString = htmlConverter.GetHTMLString(textTokens, tokenConverters);

            actualString.Should().BeEquivalentTo(expectedString);
        }

        [Test]
        public void GetHTMLString_CorrectStringLine_TextTokensAndEmTokens()
        {
            var textTokens = new List<TextToken>
            {
                new TextToken(0,2,TokenType.Text, "aa"),
                new TextToken(2,4,TokenType.Emphasized,"cccc", 
                    new List<TextToken>
                    {
                        new TextToken(0, 4, TokenType.Text, "cccc")
                    }),
                new TextToken(6,2,TokenType.Text, "bb"),
                new TextToken(9,4,TokenType.Emphasized,"dddd", 
                    new List<TextToken>
                    {
                        new TextToken(0, 4, TokenType.Text, "dddd")
                    }),
            };
            var htmlConverter = new HTMLConverter();
            var expectedString = "aa<em>cccc</em>bb<em>dddd</em>";
            
            var actualString = htmlConverter.GetHTMLString(textTokens, tokenConverters);

            actualString.Should().BeEquivalentTo(expectedString);
        }

        [Test]
        public void GetHTMLString_CorrectStringLine_EmTokenInsideStrong()
        {
            var textTokens = new List<TextToken>
            {
                new TextToken(0,2,TokenType.Strong, "aa", new List<TextToken>
                {
                    new TextToken(0,4,TokenType.Text,"cccc"),
                    new TextToken(4,2,TokenType.Emphasized,"ab", new List<TextToken>
                    {
                        new TextToken(0,2,TokenType.Text, "ab")
                    }),
                    new TextToken(6,4,TokenType.Text,"bbbb"),
                }),
                
            };
            var htmlConverter = new HTMLConverter();
            var expectedString = "<strong>cccc<em>ab</em>bbbb</strong>";
            
            var actualString = htmlConverter.GetHTMLString(textTokens, tokenConverters);

            actualString.Should().BeEquivalentTo(expectedString);
        }
    }
}