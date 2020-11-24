using System.Collections.Generic;
using FluentAssertions;
using Markdown.Converters;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class HtmlConverterTests
    {
        private IConverter converter;

        [SetUp]
        public void SetUp()
        {
            converter = new HtmlConverter(new TokenConverterFactory());
        }

        [TestCaseSource(nameof(TestCases))]
        public void ConvertTokensToHtml_ReturnExpectedResult_When(Token token, string expectedResult)
        {
            converter.ConvertTokens(new List<Token> {token}).Should().Be(expectedResult);
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(new StrongToken(0, "text", 7), "<strong>text</strong>").SetName(
                "Strong tag");
            yield return new TestCaseData(new EmphasizedToken(0, "text", 5), "<em>text</em>").SetName(
                "Emphasized tag");
            yield return new TestCaseData(new HeadingToken(0, "text", 5), "<h1>text</h1>").SetName("Heading tag");
            yield return new TestCaseData(new PlaintTextToken(0, "text", 3), "text").SetName("PlainText");
            yield return new TestCaseData(new ImageToken(0, "![text](url)", 15)
            {
                ChildTokens =
                {
                    new PlaintTextToken(2, "text", 3),
                    new PlaintTextToken(7, "url", 8)
                }
            }, @"<img src=""url"" alt=""text"">").SetName("Image");

            yield return new TestCaseData(
                new StrongToken(0, "__s _e_ s__", 0)
                {
                    ChildTokens =
                    {
                        new PlaintTextToken(2, "s ", 3),
                        new EmphasizedToken(4, "e", 6),
                        new PlaintTextToken(7, " s", 8)
                    }
                }, "<strong>s <em>e</em> s</strong>").SetName("Emphasized in strong");

            yield return new TestCaseData(
                new HeadingToken(0, "# h __s _e _E_ e_ s__", 0)
                {
                    ChildTokens =
                    {
                        new PlaintTextToken(0, "h ", 20),
                        new StrongToken(4, "s _e _E_ e_ s", 20)
                        {
                            ChildTokens =
                            {
                                new PlaintTextToken(2, "s ", 3),
                                new EmphasizedToken(4, "e _E_ e", 6)
                                {
                                    ChildTokens =
                                    {
                                        new PlaintTextToken(2, "e ", 3),
                                        new EmphasizedToken(4, "E", 6),
                                        new PlaintTextToken(7, " e", 8)
                                    }
                                },
                                new PlaintTextToken(7, " s", 8)
                            }
                        }
                    }
                }, "<h1>h <strong>s <em>e <em>E</em> e</em> s</strong></h1>").SetName("Deep nesting");
        }
    }
}