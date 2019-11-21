using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class MdTokenFixer_Should
    {
        [TestFixture]
        class MdTokenParser_Should
        {
            [Test]
            [TestCaseSource(nameof(GetTestCaseData))]
            public void FixTokensCorrectly(List<Token> tokens, List<Token> expectedResult)
            {
                var fixer = new MdTokenFixer();
                fixer.FixTokens(tokens).Should().BeEquivalentTo(expectedResult);
            }

            private static IEnumerable<TestCaseData> GetTestCaseData()
            {
                yield return new TestCaseData(new List<Token>
                    {
                        new Token(TokenType.Text, @"abc")
                    },
                    new List<Token>
                    {
                        new Token(TokenType.Text, @"abc")
                    }).SetName("text only");

                yield return new TestCaseData(new List<Token>
                    {
                        new PairToken(TokenType.Bold),
                        new Token(TokenType.Text, @"abc")
                    },
                    new List<Token>
                    {
                        new Token(TokenType.Text, "__"),
                        new Token(TokenType.Text, @"abc")
                    }).SetName("filter single bold tag");

                yield return new TestCaseData(new List<Token>
                    {
                        new Token(TokenType.Text, @"abc"),
                        new PairToken(TokenType.Bold)
                    },
                    new List<Token>
                    {
                        new Token(TokenType.Text, @"abc"),
                        new Token(TokenType.Text, "__")
                    }).SetName("filter single bold tag in the end");

                yield return new TestCaseData(new List<Token>
                    {
                        new PairToken(TokenType.Italic),
                        new Token(TokenType.Text, @"abc")
                    },
                    new List<Token>
                    {
                        new Token(TokenType.Text, "_"),
                        new Token(TokenType.Text, @"abc")
                    }).SetName("filter single italic tag");

                yield return new TestCaseData(new List<Token>
                    {
                        new Token(TokenType.Text, @"abc"),
                        new PairToken(TokenType.Italic)
                    },
                    new List<Token>
                    {
                        new Token(TokenType.Text, @"abc"),
                        new Token(TokenType.Text, "_")
                    }).SetName("filter single italic tag in the end");

                yield return new TestCaseData(new List<Token>
                    {
                        new Token(TokenType.Text, @"abc"),
                        new PairToken(TokenType.Italic),
                        new Token(TokenType.Text, @"def"),
                        new PairToken(TokenType.Italic),
                        new Token(TokenType.Text, @"ghi")
                    },
                    new List<Token>
                    {
                        new Token(TokenType.Text, @"abc"),
                        new Token(TokenType.Text, "_"),
                        new Token(TokenType.Text, @"def"),
                        new Token(TokenType.Text, "_"),
                        new Token(TokenType.Text, @"ghi")
                    }).SetName("filter italic tags in the middle of word");

                yield return new TestCaseData(new List<Token>
                    {
                        new Token(TokenType.Text, @"abc"),
                        new PairToken(TokenType.Bold),
                        new Token(TokenType.Text, @"def"),
                        new PairToken(TokenType.Bold),
                        new Token(TokenType.Text, @"ghi")
                    },
                    new List<Token>
                    {
                        new Token(TokenType.Text, @"abc"),
                        new Token(TokenType.Text, "__"),
                        new Token(TokenType.Text, @"def"),
                        new Token(TokenType.Text, "__"),
                        new Token(TokenType.Text, @"ghi")
                    }).SetName("filter bold tags in the middle of word");

                yield return new TestCaseData(new List<Token>
                    {
                        new Token(TokenType.Text, @"abc"),
                        new PairToken(TokenType.Italic),
                        new Token(TokenType.Text, @"def"),
                        new PairToken(TokenType.Italic)
                    },
                    new List<Token>
                    {
                        new Token(TokenType.Text, @"abc"),
                        new Token(TokenType.Text, "_"),
                        new Token(TokenType.Text, @"def"),
                        new Token(TokenType.Text, "_")
                    }).SetName("filter italic tags when the first is in the middle of a word");

                yield return new TestCaseData(new List<Token>
                    {
                        new PairToken(TokenType.Italic),
                        new Token(TokenType.Text, @"abc"),
                        new PairToken(TokenType.Italic),
                        new Token(TokenType.Text, @"def")
                    },
                    new List<Token>
                    {
                        new Token(TokenType.Text, "_"),
                        new Token(TokenType.Text, @"abc"),
                        new Token(TokenType.Text, "_"),
                        new Token(TokenType.Text, @"def")
                    }).SetName("filter italic tags when the second is in the middle of a word");

                yield return new TestCaseData(new List<Token>
                    {
                        new Token(TokenType.Text, @"abc"),
                        new HeaderToken(TokenType.H3),
                        new Token(TokenType.Text, @"def")
                    },
                    new List<Token>
                    {
                        new Token(TokenType.Text, @"abc"),
                        new Token(TokenType.Text, "###"),
                        new Token(TokenType.Text, @"def")
                    }).SetName("filter header tag when it is not in the beginning");

                yield return new TestCaseData(new List<Token>
                    {
                        new PairToken(TokenType.Bold),
                        new PairToken(TokenType.Italic),
                        new Token(TokenType.Text, @"abc "),
                        new Token(TokenType.Text, @"def"),
                        new PairToken(TokenType.Italic),
                        new PairToken(TokenType.Bold)
                    },
                    new List<Token>
                    {
                        new PairToken(TokenType.Bold),
                        new PairToken(TokenType.Italic),
                        new Token(TokenType.Text, @"abc "),
                        new Token(TokenType.Text, @"def"),
                        new PairToken(TokenType.Italic),
                        new PairToken(TokenType.Bold)
                    }).SetName("not filter correct tag sequence");
            }
        }
    }
}