using System.Collections.Generic;
using Markdown.SyntaxParser;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests.TokensFormatter
{
    public class TokensFormatterTestData
    {
        private static readonly TokenTreeCreationHelper TokenTreeHelper = new();

        internal static IEnumerable<TestCaseData> TestTextData()
        {
            yield return new TestCaseData(new[] {TokenTree.FromText("")}, "")
                .SetName("empty text");
            yield return new TestCaseData(new[] {TokenTree.FromText("abc")}, "abc")
                .SetName("only text");
            yield return new TestCaseData(TokenTreeHelper.GenerateFromText("abc", "\n", "xyz"),
                    "abc\nxyz")
                .SetName("text with new lines");
            yield return new TestCaseData(TokenTreeHelper.GenerateFromText("abc", "\n", "\n", "xyz"),
                    "abc\n\nxyz")
                .SetName("text with several new lines");
        }

        internal static IEnumerable<TestCaseData> TestItalicsData()
        {
            yield return new TestCaseData(new[] {new TokenTree(Token.Italics, TokenTree.FromText("abc"))},
                    "<em>abc</em>")
                .SetName("italics text");
            yield return new TestCaseData(new[]
                {
                    new TokenTree(Token.Italics, TokenTree.FromText("abc")),
                    TokenTree.FromText("a"), new TokenTree(Token.Italics, TokenTree.FromText("xyz"))
                }, "<em>abc</em>a<em>xyz</em>")
                .SetName("two bold tags in one line");
        }

        internal static IEnumerable<TestCaseData> TestBoldData()
        {
            yield return new TestCaseData(new[] {new TokenTree(Token.Bold, TokenTree.FromText("abc"))},
                    "<strong>abc</strong>")
                .SetName("bold text");
            yield return new TestCaseData(new[]
                {
                    new TokenTree(Token.Bold, TokenTree.FromText("abc")),
                    TokenTree.FromText("a"), new TokenTree(Token.Bold, TokenTree.FromText("xyz"))
                }, "<strong>abc</strong>a<strong>xyz</strong>")
                .SetName("two bold tags in one line");
            yield return new TestCaseData(new[]
                {
                    new TokenTree(Token.Bold, TokenTree.FromText("abc"),
                        new TokenTree(Token.Italics, TokenTree.FromText("xyz")), TokenTree.FromText("abc"))
                }, "<strong>abc<em>xyz</em>abc</strong>")
                .SetName("italics in bold");
        }

        internal static IEnumerable<TestCaseData> TestHeaderData()
        {
            yield return new TestCaseData(new[] {new TokenTree(Token.Header1, TokenTree.FromText("abc"))},
                    "<h1>abc</h1>")
                .SetName("one paragraph with header");
            yield return new TestCaseData(new[]
                {
                    TokenTree.FromText("\n"),
                    new TokenTree(Token.Header1, TokenTree.FromText("abc"))
                }, "\n<h1>abc</h1>")
                .SetName("header after new line");
            yield return new TestCaseData(TokenTreeHelper.GenerateFromText("xyz", "# ", "abc"),
                    "xyz# abc")
                .SetName("header after text");
            yield return new TestCaseData(new[]
                    {
                        new TokenTree(Token.Header1, new TokenTree(Token.Italics, TokenTree.FromText("abc")),
                            new TokenTree(Token.Bold, TokenTree.FromText("xyz")))
                    },
                    "<h1><em>abc</em><strong>xyz</strong></h1>")
                .SetName("header with italics and bold");
            yield return new TestCaseData(new[]
                    {
                        new TokenTree(Token.Header1, TokenTree.FromText("abc")),
                        TokenTree.FromText("\n"), new TokenTree(Token.Header1, TokenTree.FromText("abc"))
                    },
                    "<h1>abc</h1>\n<h1>abc</h1>")
                .SetName("headers in different lines");
        }
    }
}