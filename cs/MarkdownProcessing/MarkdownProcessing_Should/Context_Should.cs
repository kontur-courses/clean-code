using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace MarkdownProcessing.MarkdownProcessing_Should
{
    [TestFixture]
    public class Context_Should
    {
        [Test]
        public void Context_ShouldParsePlaintTextTag()
        {
            var expected = "<p>Hello world</p>";
            var actual = new Context("Hello world").ParseInputIntoTokens();
            actual.Should().Be(expected);
        }

        [Test]
        public void Context_ShouldParseSimpleTag()
        {
            var expected = "<strong><p>Hello world</p></strong>";
            var actual = new Context("_Hello world_").ParseInputIntoTokens();
            actual.Should().Be(expected);
        }

        [Test]
        public void ConvertToken_ShouldConvertPlainTextToken()
        {
            var context = new Context("Hello world");
            context.ConvertToken(new SimpleToken("Hello world"));
            context.output.ToString().Should().Be("Hello world");
        }

        [Test]
        public void ConvertToken_ShouldConvertSimpleToken()
        {
            var context = new Context("_Hello world_");
            context.ConvertToken(new ComplicatedToken(TokenType.Bold)
            {
                ChildTokens = new List<Token> {new SimpleToken("Hello world")}
            });
            context.output.ToString().Should().Be("<strong>Hello world</strong>");
        }

        [Test]
        public void ConvertToken_ShouldConvertTokenWithDifferentTags()
        {
            var context = new Context("#Hello world\n");
            context.ConvertToken(new ComplicatedToken(TokenType.Header1)
            {
                ChildTokens = new List<Token>
                {
                    new SimpleToken("Hello world")
                }
            });
            context.output.ToString().Should().Be("<h1>Hello world</h1>");
        }

        [Test]
        public void ConvertToken_ShouldConvertSimpleTokenIntoParent()
        {
            var context = new Context("_Hello world_");
            context.ConvertToken(new ComplicatedToken(TokenType.Parent)
            {
                ChildTokens = new List<Token>
                {
                    new ComplicatedToken(TokenType.Bold)
                    {
                        ChildTokens = new List<Token> {new SimpleToken("Hello world")}
                    }
                }
            });
            context.output.ToString().Should().Be("<p><strong>Hello world</strong></p>");
        }

        [Test]
        public void ConvertToken_ShouldConvertTwoSimpleTokensIntoParent()
        {
            var context = new Context("_*Hello* world_");
            context.ConvertToken(new ComplicatedToken(TokenType.Parent)
            {
                ChildTokens = new List<Token>
                {
                    new ComplicatedToken(TokenType.Bold)
                    {
                        ChildTokens = new List<Token>
                        {
                            new ComplicatedToken(TokenType.Italic)
                            {
                                ChildTokens = new List<Token> {new SimpleToken("Hello")}
                            },
                            new SimpleToken(" world")
                        }
                    }
                }
            });
            context.output.ToString().Should().Be("<p><strong><em>Hello</em> world</strong></p>");
        }

        [Test]
        public void ConvertToken_ShouldConvertHeaderInnerTokensIntoParent()
        {
            var context = new Context("#Hello *world*\n");
            context.ConvertToken(new ComplicatedToken(TokenType.Parent)
            {
                ChildTokens = new List<Token>
                {
                    new ComplicatedToken(TokenType.Header1)
                    {
                        ChildTokens = new List<Token>
                        {
                            new SimpleToken("Hello "),
                            new ComplicatedToken(TokenType.Bold)
                            {
                                ChildTokens = new List<Token>
                                {
                                    new SimpleToken("world")
                                }
                            }
                        }
                    }
                }
            });
            context.output.ToString().Should().Be("<p><h1>Hello <strong>world</strong></h1></p>");
        }
    }
}