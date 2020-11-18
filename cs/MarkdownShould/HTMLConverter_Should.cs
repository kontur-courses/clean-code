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
            
            var tokenConverters = new TokenConverterFactory();
            htmlConverter = new HTMLConverter(tokenConverters);
        }

        [Test]
        public void GetHTMLString_CorrectStringLine_OnlyTextTokens()
        {
            var textTokens = new List<IToken>
            {
                new PlaintTextToken("aa"),
                new PlaintTextToken("cccc")
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
                new PlaintTextToken("aa"),
                new EmphasizedTextToken("_cccc_",
                    new List<IToken>
                    {
                        new PlaintTextToken("cccc")
                    }),
                new PlaintTextToken("bb"),
                new EmphasizedTextToken("_dddd_",
                    new List<IToken>
                    {
                        new PlaintTextToken("dddd")
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
                new StrongTextToken("__aa__", new List<IToken>
                {
                    new PlaintTextToken("cccc"),
                    new EmphasizedTextToken("_ab_", new List<IToken>
                    {
                        new PlaintTextToken("ab")
                    }),
                    new PlaintTextToken("bbbb")
                })
            };
            var expectedString = "<strong>cccc<em>ab</em>bbbb</strong>";

            var actualString = htmlConverter.ConvertTokens(textTokens);

            actualString.Should().BeEquivalentTo(expectedString);
        }

        [Test]
        public void GetHTMLString_CorrectStringLine_HeaderToken()
        {
            var textTokens = new List<IToken>
            {
                new HeaderTextToken("#aa", new List<IToken>
                {
                    new PlaintTextToken("cccc"),
                    new EmphasizedTextToken("_ab_", new List<IToken>
                    {
                        new PlaintTextToken("ab")
                    }),
                    new PlaintTextToken("bbbb")
                })
            };
            var expectedString = "<h1>cccc<em>ab</em>bbbb</h1>";

            var actualString = htmlConverter.ConvertTokens(textTokens);

            actualString.Should().BeEquivalentTo(expectedString);
        }
    }
}