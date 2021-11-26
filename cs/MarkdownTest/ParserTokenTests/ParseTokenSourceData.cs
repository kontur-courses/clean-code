using System.Collections.Generic;
using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    public static class ParseTokenSourceData
    {
        public static IEnumerable<TestCaseData> Italics()
        {
            yield return new TestCaseData(
                new List<IToken> { Token.Italics, TokenText.FromText("#text") },
                new TokenTree[] { new("_"), new("#text") }).SetName("Italics doesn't close");

            yield return new TestCaseData(
                new List<IToken>
                {
                    Token.Italics, Token.FromText("text"), Token.Italics
                },
                new[]
                {
                    new TokenTree(Token.Italics, new List<TokenTree>
                    {
                        new("text")
                    })
                }).SetName("Italics cover text");

            yield return new TestCaseData(
                new List<IToken>
                {
                    Token.FromText("t"), Token.Italics, Token.FromText("12"), Token.Italics, Token.FromText("3")
                },
                new TokenTree[]
                {
                    new("t"), new("_"), new("12"), new("_"), new("3")
                }).SetName("Italics surrounded by digits");

            yield return new TestCaseData(
                new List<IToken>
                {
                    Token.FromText("t"), Token.Italics, Token.FromText("a"), Token.Italics
                },
                new TokenTree[]
                {
                    new("t"), new(new TokenItalics(), new List<TokenTree> { new(Token.FromText("a")) })
                }).SetName("Italics with word part");

            yield return new TestCaseData(
                new List<IToken>
                {
                    Token.FromText("t"), Token.Italics, Token.FromText(" "), Token.FromText("a"), Token.Italics
                },
                new TokenTree[]
                {
                    new("t"), new("_"), new("12"), new("_"), new("a")
                }).SetName("Italics don't work in different words");

            yield return new TestCaseData(
                new List<IToken>
                {
                    Token.Italics, Token.FromText(" t"), Token.Italics
                },
                new TokenTree[]
                {
                    new("_"), new(" t"), new("_")
                }).SetName("Italics must be followed by a non-whitespace character");
        }

        public static IEnumerable<TestCaseData> ItalicsAndStrong()
        {
            yield return new TestCaseData(
                new List<IToken>
                {
                    Token.Header1, Token.Strong, TokenText.FromText("text"), Token.Strong,
                    Token.Italics, TokenText.FromText("text"), Token.Italics, Token.NewLine
                },
                new TokenTree[]
                {
                    new(new TokenHeader1(),
                        new List<TokenTree>
                        {
                            new(Token.Strong,
                                new List<TokenTree>
                                {
                                    new("text")
                                }),
                            new(Token.Italics,
                                new List<TokenTree>
                                {
                                    new("text")
                                })
                        }),
                    new(Token.FromText("\n"))
                }).SetName("Strong and italics are close to each other");

            yield return new TestCaseData(
                new List<IToken>
                {
                    Token.FromText("a"), Token.Italics, Token.FromText("b"), Token.Strong, Token.FromText("a"),
                    Token.Italics, Token.FromText("c"), Token.Strong
                },
                new TokenTree[]
                {
                    new("a"), new("_b__a_"), new("c"), new("__")
                }).SetName("italics intersects with strong");

            yield return new TestCaseData(
                new List<IToken>
                {
                    Token.Italics, Token.FromText(""), Token.Italics
                },
                new TokenTree[]
                {
                    new("_"), new(""), new("_")
                }).SetName("italics intersects with strong");
        }

        public static IEnumerable<TestCaseData> Strong()
        {
            yield return new TestCaseData(
                new List<IToken> { Token.Strong, TokenText.FromText("#text") },
                new TokenTree[] { new("__"), new("#text") }).SetName("Italics doesn't close");

            yield return new TestCaseData(
                new List<IToken>
                {
                    Token.FromText("t"), Token.Strong, Token.FromText("12"), Token.Strong, Token.FromText("3")
                },
                new TokenTree[]
                {
                    new("t"), new("__"), new("12"), new("__"), new("3")
                }).SetName("Strong surrounded by digits");

            yield return new TestCaseData(
                new List<IToken>
                {
                    Token.FromText("t"), Token.Strong, Token.FromText(" "), Token.FromText("a"), Token.Strong
                },
                new TokenTree[]
                {
                    new("t"), new("__"), new("12"), new("__"), new("a")
                }).SetName("Strong don't work in different words");

            yield return new TestCaseData(
                new List<IToken>
                {
                    Token.Strong, Token.FromText("text"), Token.Strong
                },
                new[]
                {
                    new TokenTree(Token.Strong, new List<TokenTree>
                    {
                        new("text")
                    })
                }).SetName("Strong cover text");
        }
    }
}