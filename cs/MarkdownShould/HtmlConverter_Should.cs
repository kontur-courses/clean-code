using System.Collections.Generic;
using FluentAssertions;
using Markdown.Converters;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class HtmlConverter_Should
    {
        private HtmlConverter htmlConverter;

        [SetUp]
        public void SetUp()
        {
            var tokenConverters = new TokenConverterFactory();
            htmlConverter = new HtmlConverter(tokenConverters);
        }

        [Test]
        public void CorrectStringLine_OnlyPlainTextTokens()
        {
            var textTokens = new List<IToken>
            {
                new PlainTextToken("aa"),
                new PlainTextToken("cccc")
            };
            var expectedString = "aacccc";

            var actualString = htmlConverter.ConvertTokens(textTokens);

            actualString.Should().BeEquivalentTo(expectedString);
        }

        [Test]
        public void CorrectStringLine_PlainTextTokensAndEmphasizedTextTokens()
        {
            var textTokens = new List<TextToken>
            {
                new PlainTextToken("aa"),
                new EmphasizedTextToken("_cccc_") {SubTokens = new List<IToken> {new PlainTextToken("cccc")}},
                new PlainTextToken("bb"),
                new EmphasizedTextToken("_dddd_") {SubTokens = new List<IToken> {new PlainTextToken("dddd")}}
            };
            var expectedString = "aa<em>cccc</em>bb<em>dddd</em>";

            var actualString = htmlConverter.ConvertTokens(textTokens);

            actualString.Should().BeEquivalentTo(expectedString);
        }

        [Test]
        public void CorrectStringLine_EmphasizedTextTokenInsideStrongTextToken()
        {
            var textTokens = new List<TextToken>
            {
                new StrongTextToken("__aa__")
                {
                    SubTokens = new List<IToken>
                    {
                        new PlainTextToken("cccc"),
                        new EmphasizedTextToken("_ab_") {SubTokens = new List<IToken> {new PlainTextToken("ab")}},
                        new PlainTextToken("bbbb")
                    }
                }
            };
            var expectedString = "<strong>cccc<em>ab</em>bbbb</strong>";

            var actualString = htmlConverter.ConvertTokens(textTokens);

            actualString.Should().BeEquivalentTo(expectedString);
        }

        [Test]
        public void CorrectStringLine_HeaderTextToken()
        {
            var textTokens = new List<IToken>
            {
                new HeaderTextToken("#aa")
                {
                    SubTokens = new List<IToken>
                    {
                        new PlainTextToken("cccc"),
                        new EmphasizedTextToken("_ab_")
                        {
                            SubTokens = new List<IToken>
                            {
                                new PlainTextToken("ab")
                            }
                        },
                        new PlainTextToken("bbbb")
                    }
                }
            };
            var expectedString = "<h1>cccc<em>ab</em>bbbb</h1>";

            var actualString = htmlConverter.ConvertTokens(textTokens);

            actualString.Should().BeEquivalentTo(expectedString);
        }
    }
}