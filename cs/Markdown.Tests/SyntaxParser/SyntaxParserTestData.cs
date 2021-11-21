using System.Collections.Generic;
using System.Linq;
using Markdown.SyntaxParser;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests.SyntaxParser
{
    public class SyntaxParserTestData
    {
        private static readonly TokenTreeCreationHelper TokenTreeHelper = new();

        public static IEnumerable<TestCaseData> ItalicsData()
        {
            yield return new TestCaseData(new[] {Token.Italics, Token.Text("abc"), Token.Italics},
                    new[] {new TokenTree(Token.Italics, TokenTree.FromText("abc"))})
                .SetName("italics in the beginning");
            yield return new TestCaseData(
                    new[] {Token.Text("a"), Token.Italics, Token.Text("abc"), Token.Italics, Token.Text("a")},
                    new[]
                    {
                        TokenTree.FromText("a"), new TokenTree(Token.Italics,
                            TokenTree.FromText("abc")),
                        TokenTree.FromText("a")
                    })
                .SetName("italics in the middle");
            yield return new TestCaseData(new[] {Token.Text("a"), Token.Italics, Token.Text("abc"), Token.Italics},
                    new[]
                    {
                        TokenTree.FromText("a"), new TokenTree(Token.Italics, TokenTree.FromText("abc"))
                    })
                .SetName("italics in the end");

            yield return new TestCaseData(
                    new[] {Token.Text("a"), Token.Italics, Token.Text(" abc"), Token.Italics, Token.Text("a")},
                    TokenTreeHelper.GenerateFromText("a", "_", " abc", "_", "a"))
                .SetName("italics with space after opening tag");
            yield return new TestCaseData(
                    new[] {Token.Text("a"), Token.Italics, Token.Text("abc "), Token.Italics, Token.Text("a")},
                    TokenTreeHelper.GenerateFromText("a", "_", "abc ", "_", "a"))
                .SetName("italics with space before closing tag");

            yield return new TestCaseData(new[]
                    {
                        Token.Text("a"), Token.Italics, Token.Text("abc"), Token.Italics, Token.Text("a"),
                        Token.Italics, Token.Text("xyz"), Token.Italics, Token.Text("a")
                    },
                    new[]
                    {
                        TokenTree.FromText("a"), new TokenTree(Token.Italics, TokenTree.FromText("abc")),
                        TokenTree.FromText("a"), new TokenTree(Token.Italics, TokenTree.FromText("xyz")),
                        TokenTree.FromText("a")
                    })
                .SetName("several italics tags in one paragraph");

            yield return new TestCaseData(new[]
                    {
                        Token.Italics, Token.Text("a"), Token.Bold, Token.Text("b"), Token.Bold,
                        Token.Text("c"), Token.Italics
                    },
                    new TokenTree[]
                        {new(Token.Italics, TokenTreeHelper.GenerateFromText("a", "__", "b", "__", "c"))})
                .SetName("bold in italics");

            yield return new TestCaseData(
                    new[]
                    {
                        Token.Text("a"), Token.Italics, Token.Text("b"), Token.Bold, Token.Text("a"),
                        Token.Italics, Token.Text("c"), Token.Bold
                    }, TokenTreeHelper.GenerateFromText("a", "_b__a_c__"))
                .SetName("italics intersects with bold");

            yield return new TestCaseData(
                    new[] {Token.Text("a"), Token.Italics, Token.Text("123"), Token.Italics, Token.Text("a")},
                    TokenTreeHelper.GenerateFromText("a", "_", "123", "_", "a"))
                .SetName("italics surrounded with digits");

            yield return new TestCaseData(
                    new[] {Token.Text("a"), Token.Italics, Token.Text("abc"), Token.Italics, Token.Text("a")},
                    new[]
                    {
                        TokenTree.FromText("a"), new(Token.Italics, TokenTree.FromText("abc")),
                        TokenTree.FromText("a")
                    })
                .SetName("italics marks only part of the word");

            yield return new TestCaseData(
                    new[] {Token.Text("a"), Token.Italics, Token.Text("uncle bob"), Token.Italics, Token.Text("a")},
                    TokenTreeHelper.GenerateFromText("a", "_", "uncle bob", "_", "a"))
                .SetName("italics tags in different words");

            yield return new TestCaseData(new[] {Token.Italics, Token.Italics},
                    TokenTreeHelper.GenerateFromText("_", "_"))
                .SetName("italics without text");
        }

        public static IEnumerable<TestCaseData> BoldData()
        {
            yield return new TestCaseData(new[] {Token.Bold, Token.Text("abc"), Token.Bold},
                    new[] {new TokenTree(Token.Bold, TokenTree.FromText("abc"))})
                .SetName("bold in the beginning");
            yield return new TestCaseData(
                    new[] {Token.Text("a"), Token.Bold, Token.Text("abc"), Token.Bold, Token.Text("a")},
                    new[]
                    {
                        TokenTree.FromText("a"), new TokenTree(Token.Bold,
                            TokenTree.FromText("abc")),
                        TokenTree.FromText("a")
                    })
                .SetName("bold in the middle");
            yield return new TestCaseData(new[] {Token.Text("a"), Token.Bold, Token.Text("abc"), Token.Bold},
                    new[]
                    {
                        TokenTree.FromText("a"), new TokenTree(Token.Bold, TokenTree.FromText("abc"))
                    })
                .SetName("bold in the end");

            yield return new TestCaseData(
                    new[] {Token.Text("a"), Token.Bold, Token.Text(" abc"), Token.Bold, Token.Text("a")},
                    TokenTreeHelper.GenerateFromText("a", "__", " abc", "__", "a"))
                .SetName("bold with space after opening tag");
            yield return new TestCaseData(
                    new[] {Token.Text("a"), Token.Bold, Token.Text("abc "), Token.Bold, Token.Text("a")},
                    TokenTreeHelper.GenerateFromText("a", "__", "abc ", "__", "a"))
                .SetName("bold with space before closing tag");

            yield return new TestCaseData(new[]
                    {
                        Token.Text("a"), Token.Bold, Token.Text("abc"), Token.Bold, Token.Text("a"),
                        Token.Bold, Token.Text("xyz"), Token.Bold, Token.Text("a")
                    },
                    new[]
                    {
                        TokenTree.FromText("a"), new TokenTree(Token.Bold, TokenTree.FromText("abc")),
                        TokenTree.FromText("a"), new TokenTree(Token.Bold, TokenTree.FromText("xyz")),
                        TokenTree.FromText("a")
                    })
                .SetName("several bold tags in one paragraph");

            yield return new TestCaseData(new[]
                    {
                        Token.Bold, Token.Text("a"), Token.Italics, Token.Text("b"), Token.Italics,
                        Token.Text("c"), Token.Bold
                    },
                    new[]
                    {
                        new TokenTree(Token.Bold, TokenTree.FromText("a"),
                            new TokenTree(Token.Italics, TokenTree.FromText("b")),
                            TokenTree.FromText("c"))
                    })
                .SetName("italics in bold");

            yield return new TestCaseData(
                    new[]
                    {
                        Token.Text("a"), Token.Bold, Token.Text("b"), Token.Italics, Token.Text("a"),
                        Token.Bold, Token.Text("c"), Token.Italics
                    }, TokenTreeHelper.GenerateFromText("a", "__b_a__c_"))
                .SetName("bold intersects with italics");

            yield return new TestCaseData(
                    new[] {Token.Text("a"), Token.Bold, Token.Text("123"), Token.Bold, Token.Text("a")},
                    TokenTreeHelper.GenerateFromText("a", "__", "123", "__", "a"))
                .SetName("bold surrounded with digits");

            yield return new TestCaseData(
                    new[] {Token.Text("a"), Token.Bold, Token.Text("uncle bob"), Token.Bold, Token.Text("a")},
                    TokenTreeHelper.GenerateFromText("a", "__", "uncle bob", "__", "a"))
                .SetName("bold tags in different words");

            yield return new TestCaseData(new[] {Token.Bold, Token.Bold}, TokenTreeHelper.GenerateFromText("__", "__"))
                .SetName("bold without text");
        }

        public static IEnumerable<TestCaseData> EscapeSymbolData()
        {
            yield return new TestCaseData(new[] {Token.Escape},
                    new[] {TokenTree.FromText("\\")})
                .SetName("only escape tag");
            yield return new TestCaseData(new[] {Token.Escape, Token.Italics, Token.Text("abc"), Token.Italics},
                    TokenTreeHelper.GenerateFromText("_", "abc", "_"))
                .SetName("shielding italics in the beginning");
            yield return new TestCaseData(new[] {Token.Italics, Token.Text("abc"), Token.Escape, Token.Italics},
                    new[] {TokenTree.FromText("_abc_")})
                .SetName("shielding italics in the end");
            yield return new TestCaseData(
                    new[] {Token.Italics, Token.Text("abc"), Token.Escape, Token.Italics, Token.Italics},
                    new[] {new TokenTree(Token.Italics, TokenTreeHelper.GenerateFromText("abc", "_"))})
                .SetName("shielding italics in the middle");
            yield return new TestCaseData(new[] {Token.Escape, Token.Bold, Token.Text("abc"), Token.Bold},
                    TokenTreeHelper.GenerateFromText("__", "abc", "__"))
                .SetName("shielding bold");
            yield return new TestCaseData(new[] {Token.Escape, Token.Escape},
                    new[] {TokenTree.FromText("\\")})
                .SetName("shielding itself");
            yield return new TestCaseData(new[] {Token.Escape, Token.Escape, Token.Escape},
                    TokenTreeHelper.GenerateFromText("\\", "\\"))
                .SetName("shielding itself three times");
            yield return new TestCaseData(new[] {Token.Escape, Token.Escape, Token.Escape, Token.Escape},
                    TokenTreeHelper.GenerateFromText("\\", "\\"))
                .SetName("shielding itself four times");
            yield return new TestCaseData(new[]
                {
                    Token.Text("another"), Token.Text("br"), Token.Escape, Token.Text("ick"),
                    Token.Escape, Token.Text("in the"), Token.Escape, Token.Text("wall"), Token.Escape
                }, TokenTreeHelper.GenerateFromText("another", "br", "\\", "ick", "\\", "in the", "\\", "wall", "\\"))
                .SetName("shielding text");
            yield return new TestCaseData(
                    new[] {Token.Escape, Token.Escape, Token.Italics, Token.Text("abc"), Token.Italics},
                    new[]
                    {
                        TokenTree.FromText("\\"), new TokenTree(Token.Italics, TokenTree.FromText("abc"))
                    })
                .SetName("shielding itself before italics");
        }

        public static IEnumerable<TestCaseData> Header1Data()
        {
            yield return new TestCaseData(new[] {Token.Header1},
                    new[] {TokenTree.FromText("")})
                .SetName("only header tag");
            yield return new TestCaseData(new[] {Token.Header1, Token.Text("abc")},
                    new[] {new TokenTree(Token.Header1, TokenTree.FromText("abc"))})
                .SetName("header in first line");
            yield return new TestCaseData(new[] {Token.NewLine, Token.Header1, Token.Text("abc")},
                    new[]
                    {
                        TokenTree.FromText("\n"), new TokenTree(Token.Header1, TokenTree.FromText("abc"))
                    })
                .SetName("header after new line");
            yield return new TestCaseData(new[] {Token.Header1, Token.Text("abc"), Token.NewLine},
                    new[]
                    {
                        new TokenTree(Token.Header1, TokenTree.FromText("abc")), TokenTree.FromText("\n")
                    })
                .SetName("new line after header");
            yield return new TestCaseData(new[] {Token.Text("xyz"), Token.Header1, Token.Text("abc")},
                    TokenTreeHelper.GenerateFromText("xyz", "# ", "abc"))
                .SetName("header after text");
            yield return new TestCaseData(new[] {Token.Italics, Token.Header1, Token.Text("abc")},
                    TokenTreeHelper.GenerateFromText("_", "# ", "abc"))
                .SetName("header after italics");
            yield return new TestCaseData(new[] {Token.Header1, Token.Text("abc"), Token.NewLine, Token.Text("abc")},
                    new[]
                    {
                        new TokenTree(Token.Header1, TokenTree.FromText("abc")),
                        TokenTree.FromText("\n"),
                        TokenTree.FromText("abc")
                    })
                .SetName("text after header paragraph");
            yield return new TestCaseData(new[] {Token.Header1, Token.Header1, Token.Text("abc")},
                    new[] {new TokenTree(Token.Header1, TokenTreeHelper.GenerateFromText("# ", "abc"))})
                .SetName("two headers in one line");
            yield return new TestCaseData(new[] {Token.Header1, Token.Italics, Token.Text("abc"), Token.Italics},
                    new[]
                    {
                        new TokenTree(Token.Header1,
                            new TokenTree(Token.Italics, TokenTree.FromText("abc")))
                    })
                .SetName("italics in header");
        }

        public static IEnumerable<TestCaseData> ShouldNotBeApplied()
        {
            yield return new TestCaseData(Enumerable.Empty<Token>(), Enumerable.Empty<TokenTree>())
                .SetName("No tokens");
            yield return new TestCaseData(new[] {Token.Text("abc cde")}, new[] {TokenTree.FromText("abc cde")})
                .SetName("Only text tokens");
            yield return new TestCaseData(new[] {Token.Text("abc"), Token.NewLine, Token.Text("zyx")},
                    TokenTreeHelper.GenerateFromText("abc", "\n", "zyx"))
                .SetName("Text tokens with NewLine");
        }
    }
}