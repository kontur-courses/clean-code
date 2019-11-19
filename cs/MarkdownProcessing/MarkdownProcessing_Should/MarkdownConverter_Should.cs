using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace MarkdownProcessing.MarkdownProcessing_Should
{
    [TestFixture]
    public class MarkdownConverter_Should
    {
        [Test]
        public void ConvertToken_ShouldConvertPlainTextToken()
        {
            var converter = new MarkdownConverter(new SimpleToken("Hello world"));
            converter.ConvertToHtml().Should().Be("Hello world");
        }

        [Test]
        public void ConvertToken_ShouldConvertSimpleToken()
        {
            var converter = new MarkdownConverter(new ComplicatedToken(TokenType.Bold)
            {
                ChildTokens = new List<Token> {new SimpleToken("Hello world")}
            });
            converter.ConvertToHtml().Should().Be("<strong>Hello world</strong>");
        }

        [Test]
        public void ConvertToken_ShouldConvertTokenWithDifferentTags()
        {
            var converter = new MarkdownConverter(new ComplicatedToken(TokenType.Header1)
            {
                ChildTokens = new List<Token>
                {
                    new SimpleToken("Hello world")
                }
            });
            converter.ConvertToHtml().Should().Be("<h1>Hello world</h1>");
        }

        [Test]
        public void ConvertToken_ShouldConvertSimpleTokenIntoParent()
        {
            var converter = new MarkdownConverter(new ComplicatedToken(TokenType.Parent)
            {
                ChildTokens = new List<Token>
                {
                    new SimpleToken("Hello world")
                }
            });
            converter.ConvertToHtml().Should().Be("<p>Hello world</p>");
        }

        [Test]
        public void ConvertToken_ShouldConvertComplicatedTokenIntoParent()
        {
            var converter = new MarkdownConverter(new ComplicatedToken(TokenType.Parent)
            {
                ChildTokens = new List<Token>
                {
                    new ComplicatedToken(TokenType.Bold)
                    {
                        ChildTokens = new List<Token> {new SimpleToken("Hello world")}
                    }
                }
            });
            converter.ConvertToHtml().Should().Be("<p><strong>Hello world</strong></p>");
        }

        [Test]
        public void ConvertToken_ShouldConvertTwoSimpleTokensIntoParent()
        {
            var converter = new MarkdownConverter(new ComplicatedToken(TokenType.Parent)
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
            converter.ConvertToHtml().Should().Be("<p><strong><em>Hello</em> world</strong></p>");
        }

        [Test]
        public void ConvertToken_ShouldConvertHeaderInnerTokensIntoParent()
        {
            var converter = new MarkdownConverter(new ComplicatedToken(TokenType.Parent)
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
            converter.ConvertToHtml().Should().Be("<p><h1>Hello <strong>world</strong></h1></p>");
        }
    }
}