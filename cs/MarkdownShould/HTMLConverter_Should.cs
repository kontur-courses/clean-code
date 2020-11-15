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
    }
}