using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class SemanticAnalyzerTests
    {
        private readonly SemanticAnalyzer analyzer = new();

        private static readonly Token[][] OnlyText =
        {
            new[] { new Token(0, "aboba", TokenType.Text) },
            new[]
            {
                new Token(0, "abo", TokenType.Text),
                new Token(3, " ", TokenType.WhiteSpace),
                new Token(4, "ba", TokenType.Text)
            },
            new[] { new Token(0, "", TokenType.Text) },
        };

        private static readonly (Token[] source, Token[] expected)[] WithSentenceModiferTags =
        {
            (new[]
                {
                    new Token(0, "#", TokenType.Tag),
                    new Token(1, "header", TokenType.Text)
                },
                new[]
                {
                    new Token(0, "#", TokenType.Tag),
                    new Token(1, "header", TokenType.Text)
                }),

            (new[]
                {
                    new Token(0, "h", TokenType.Text),
                    new Token(1, "#", TokenType.Tag),
                    new Token(2, "eader", TokenType.Text)
                },
                new[]
                {
                    new Token(0, "h", TokenType.Text),
                    new Token(1, "#", TokenType.Text),
                    new Token(2, "eader", TokenType.Text)
                }),

            (new[]
                {
                    new Token(0, "header", TokenType.Text),
                    new Token(6, "#", TokenType.Tag),
                },
                new[]
                {
                    new Token(0, "header", TokenType.Text),
                    new Token(6, "#", TokenType.Text),
                }),

            (new[]
                {
                    new Token(0, "header", TokenType.Text),
                    new Token(6, " ", TokenType.WhiteSpace),
                    new Token(7, "#", TokenType.Tag),
                    new Token(8, "header", TokenType.Text)
                },
                new[]
                {
                    new Token(0, "header", TokenType.Text),
                    new Token(6, " ", TokenType.WhiteSpace),
                    new Token(7, "#", TokenType.Text),
                    new Token(8, "header", TokenType.Text)
                }),
        };

        private static readonly (Token[] source, AnalyzedToken[] expected)[] TokensSentencesWithDoubleTags =
        {
            (new[]
                {
                    new Token(0, "_", TokenType.Tag),
                    new Token(1, "aboba", TokenType.Text),
                    new Token(6, "_", TokenType.Tag)
                },
                new[]
                {
                    new AnalyzedToken(0, "_", TokenType.Tag, AnalyzedTokenType.Opener),
                    new AnalyzedToken(1, "aboba", TokenType.Text),
                    new AnalyzedToken(6, "_", TokenType.Tag, AnalyzedTokenType.Closing)
                }),

            (new[]
                {
                    new Token(0, "_", TokenType.Tag),
                    new Token(1, "aboba", TokenType.Text),
                },
                new[]
                {
                    new AnalyzedToken(0, "_", TokenType.Text),
                    new AnalyzedToken(1, "aboba", TokenType.Text),
                }),

            (new[]
                {
                    new Token(0, "__", TokenType.Tag),
                    new Token(2, "aboba", TokenType.Text),
                    new Token(7, "_", TokenType.Tag),
                    new Token(8, "biba", TokenType.Text),
                    new Token(12, "_", TokenType.Tag),
                    new Token(0, "__", TokenType.Tag),
                },
                new[]
                {
                    new AnalyzedToken(0, "__", TokenType.Tag, AnalyzedTokenType.Opener),
                    new AnalyzedToken(2, "aboba", TokenType.Text),
                    new AnalyzedToken(7, "_", TokenType.Tag, AnalyzedTokenType.Opener),
                    new AnalyzedToken(8, "biba", TokenType.Text),
                    new AnalyzedToken(12, "_", TokenType.Tag, AnalyzedTokenType.Closing),
                    new AnalyzedToken(0, "__", TokenType.Tag, AnalyzedTokenType.Closing),
                }),

            (new[]
                {
                    new Token(0, "#", TokenType.Tag),
                    new Token(1, "__", TokenType.Tag),
                    new Token(3, "aboba", TokenType.Text),
                    new Token(8, " ", TokenType.WhiteSpace),
                    new Token(8, "_", TokenType.Tag),
                    new Token(9, "biba", TokenType.Text),
                    new Token(13, "_", TokenType.Tag),
                    new Token(14, "__", TokenType.Tag),
                },
                new[]
                {
                    new AnalyzedToken(0, "#", TokenType.Tag, AnalyzedTokenType.SentenceModifer),
                    new AnalyzedToken(1, "__", TokenType.Tag, AnalyzedTokenType.Opener),
                    new AnalyzedToken(3, "aboba", TokenType.Text),
                    new AnalyzedToken(8, " ", TokenType.WhiteSpace),
                    new AnalyzedToken(8, "_", TokenType.Tag, AnalyzedTokenType.Opener),
                    new AnalyzedToken(9, "biba", TokenType.Text),
                    new AnalyzedToken(13, "_", TokenType.Tag, AnalyzedTokenType.Closing),
                    new AnalyzedToken(14, "__", TokenType.Tag, AnalyzedTokenType.Closing),
                }),
        };

        private static readonly (Token[] source, AnalyzedToken[] expected)[] TokensWithTagsInsideWords =
        {
            (new[]
                {
                    new Token(0, "#", TokenType.Tag),
                    new Token(3, "a", TokenType.Text),
                    new Token(8, "__", TokenType.Tag),
                    new Token(9, "bi", TokenType.Text),
                    new Token(10, " ", TokenType.WhiteSpace),
                    new Token(9, "ba", TokenType.Text),
                    new Token(13, "__", TokenType.Tag),
                },
                new[]
                {
                    new AnalyzedToken(0, "#", TokenType.Tag, AnalyzedTokenType.SentenceModifer),
                    new AnalyzedToken(3, "a", TokenType.Text),
                    new AnalyzedToken(8, "__", TokenType.Text),
                    new AnalyzedToken(9, "bi", TokenType.Text),
                    new AnalyzedToken(10, " ", TokenType.WhiteSpace),
                    new AnalyzedToken(9, "ba", TokenType.Text),
                    new AnalyzedToken(13, "__", TokenType.Text),
                }),
            (new[]
                {
                    new Token(1, "__", TokenType.Tag),
                    new Token(3, "aboba", TokenType.Text),
                    new Token(4, " ", TokenType.WhiteSpace),
                    new Token(9, "biba", TokenType.Text),
                    new Token(14, "__", TokenType.Tag),
                    new Token(16, "s", TokenType.Text)
                },
                new[]
                {
                    new AnalyzedToken(1, "__", TokenType.Text),
                    new AnalyzedToken(3, "aboba", TokenType.Text),
                    new AnalyzedToken(4, " ", TokenType.WhiteSpace),
                    new AnalyzedToken(9, "biba", TokenType.Text),
                    new AnalyzedToken(14, "__", TokenType.Text),
                    new AnalyzedToken(16, "s", TokenType.Text)
                }),

            (new[]
                {
                    new Token(0, "#", TokenType.Tag),
                    new Token(3, "a", TokenType.Text),
                    new Token(8, "__", TokenType.Tag),
                    new Token(9, "bi", TokenType.Text),
                    new Token(13, "__", TokenType.Tag),
                    new Token(9, "ba", TokenType.Text),
                },
                new[]
                {
                    new AnalyzedToken(0, "#", TokenType.Tag, AnalyzedTokenType.SentenceModifer),
                    new AnalyzedToken(3, "a", TokenType.Text),
                    new AnalyzedToken(8, "__", TokenType.Tag, AnalyzedTokenType.Opener),
                    new AnalyzedToken(9, "bi", TokenType.Text),
                    new AnalyzedToken(13, "__", TokenType.Tag, AnalyzedTokenType.Closing),
                    new AnalyzedToken(9, "ba", TokenType.Text)
                }),
            (new[]
                {
                    new Token(0, "#", TokenType.Tag),
                    new Token(3, "a", TokenType.Text),
                    new Token(8, "__", TokenType.Tag),
                    new Token(9, "bi", TokenType.Text),
                    new Token(10, " ", TokenType.WhiteSpace),
                    new Token(8, "__", TokenType.Tag),
                    new Token(9, "ba", TokenType.Text),
                    new Token(8, "__", TokenType.Tag)
                },
                new[]
                {
                    new AnalyzedToken(0, "#", TokenType.Tag, AnalyzedTokenType.SentenceModifer),
                    new AnalyzedToken(3, "a", TokenType.Text),
                    new AnalyzedToken(8, "__", TokenType.Text),
                    new AnalyzedToken(9, "bi", TokenType.Text),
                    new AnalyzedToken(10, " ", TokenType.WhiteSpace),
                    new AnalyzedToken(8, "__", TokenType.Tag, AnalyzedTokenType.Opener),
                    new AnalyzedToken(9, "ba", TokenType.Text),
                    new AnalyzedToken(8, "__", TokenType.Tag, AnalyzedTokenType.Closing)
                })
        };

        private static readonly (Token[] source, AnalyzedToken[] expected)[] TokensWithTagsNearDigits =
        {
            (new[]
                {
                    new Token(0, "_", TokenType.Tag),
                    new Token(1, "1", TokenType.Text),
                    new Token(2, "_", TokenType.Tag)
                },
                new[]
                {
                    new AnalyzedToken(0, "_", TokenType.Text),
                    new AnalyzedToken(1, "1", TokenType.Text),
                    new AnalyzedToken(2, "_", TokenType.Text)
                }),
            (new[]
                {
                    new Token(0, "_", TokenType.Tag),
                    new Token(1, "4el", TokenType.Text),
                    new Token(4, "_", TokenType.Tag)
                },
                new[]
                {
                    new AnalyzedToken(0, "_", TokenType.Text),
                    new AnalyzedToken(1, "4el", TokenType.Text),
                    new AnalyzedToken(4, "_", TokenType.Text)
                }),
            (new[]
                {
                    new Token(0, "_", TokenType.Tag),
                    new Token(1, "4el", TokenType.Text),
                    new Token(4, "_", TokenType.Tag),
                    new Token(5, "ti", TokenType.Text),
                    new Token(7, "_", TokenType.Tag)
                },
                new[]
                {
                    new AnalyzedToken(0, "_", TokenType.Text),
                    new AnalyzedToken(1, "4el", TokenType.Text),
                    new AnalyzedToken(4, "_", TokenType.Tag, AnalyzedTokenType.Opener),
                    new AnalyzedToken(5, "ti", TokenType.Text),
                    new AnalyzedToken(7, "_", TokenType.Tag, AnalyzedTokenType.Closing)
                }
            )
        };

        private static readonly (Token[] source, AnalyzedToken[] expected)[] TokensWithIncorrectWhiteSpaces =
        {
            (new[]
                {
                    new Token(0, "_", TokenType.Tag),
                    new Token(1, " ", TokenType.WhiteSpace),
                    new Token(1, "d", TokenType.Text),
                    new Token(2, "_", TokenType.Tag)
                },
                new[]
                {
                    new AnalyzedToken(0, "_", TokenType.Text),
                    new AnalyzedToken(1, " ", TokenType.WhiteSpace),
                    new AnalyzedToken(1, "d", TokenType.Text),
                    new AnalyzedToken(2, "_", TokenType.Text)
                }),
            (new[]
                {
                    new Token(0, "_", TokenType.Tag),
                    new Token(1, "d", TokenType.Text),
                    new Token(1, " ", TokenType.WhiteSpace),
                    new Token(2, "_", TokenType.Tag)
                },
                new[]
                {
                    new AnalyzedToken(0, "_", TokenType.Text),
                    new AnalyzedToken(1, "d", TokenType.Text),
                    new AnalyzedToken(1, " ", TokenType.WhiteSpace),
                    new AnalyzedToken(2, "_", TokenType.Text)
                })
        };

        [Test]
        public void Analise_doubleUnderlinesInsideSingleUnderlines_doubleUnderlinesShouldBeText()
        {
            var source = new[]
            {
                new Token(0, "_", TokenType.Tag),
                new Token(1, "a", TokenType.Text),
                new Token(3, " ", TokenType.WhiteSpace),
                new Token(4, "__", TokenType.Tag),
                new Token(6, "b", TokenType.Text),
                new Token(7, "__", TokenType.Tag),
                new Token(8," ",TokenType.WhiteSpace),
                new Token(9, "c", TokenType.Text),
                new Token(10, "_", TokenType.Tag)
            };

            var expected = new[]
            {
                new AnalyzedToken(0, "_", TokenType.Tag, AnalyzedTokenType.Opener),
                new AnalyzedToken(1, "a", TokenType.Text),
                new AnalyzedToken(3, " ", TokenType.WhiteSpace),
                new AnalyzedToken(4, "__", TokenType.Text),
                new AnalyzedToken(6, "b", TokenType.Text),
                new AnalyzedToken(7, "__", TokenType.Text),
                new AnalyzedToken(8," ",TokenType.WhiteSpace),
                new AnalyzedToken(9, "c", TokenType.Text),
                new AnalyzedToken(10, "_", TokenType.Tag, AnalyzedTokenType.Closing)
            };

            var actual = analyzer.Analise(source);

            actual.Should().Equal(expected);
        }

        [Test]
        public void Analise_singleUnderlinesInsideDoubleUnderlines_singleUnderlinesShouldBeTags()
        {
            var source = new[]
            {
                new Token(0, "__", TokenType.Tag),
                new Token(1, "a", TokenType.Text),
                new Token(3, " ", TokenType.WhiteSpace),
                new Token(4, "_", TokenType.Tag),
                new Token(6, "b", TokenType.Text),
                new Token(7, "_", TokenType.Tag),
                new Token(8," ",TokenType.WhiteSpace),
                new Token(9, "c", TokenType.Text),
                new Token(10, "__", TokenType.Tag)
            };

            var expected = new[]
            {
                new AnalyzedToken(0, "__", TokenType.Tag, AnalyzedTokenType.Opener),
                new AnalyzedToken(1, "a", TokenType.Text),
                new AnalyzedToken(3, " ", TokenType.WhiteSpace),
                new AnalyzedToken(4, "_", TokenType.Tag, AnalyzedTokenType.Opener),
                new AnalyzedToken(6, "b", TokenType.Text),
                new AnalyzedToken(7, "_", TokenType.Tag, AnalyzedTokenType.Closing),
                new AnalyzedToken(8," ",TokenType.WhiteSpace),
                new AnalyzedToken(9, "c", TokenType.Text),
                new AnalyzedToken(10, "__", TokenType.Tag, AnalyzedTokenType.Closing)
            };

            var actual = analyzer.Analise(source);

            actual.Should().Equal(expected);
        }

        [TestCaseSource(nameof(OnlyText))]
        public void Analise_onlyTextAndWhiteSpace_shouldNotChangeTokens(Token[] expectedTokens)
        {
            var actual = analyzer.Analise(expectedTokens)
                .Select(t => (Token)t).ToArray();

            actual.Should().Equal(expectedTokens);
        }

        [TestCaseSource(nameof(WithSentenceModiferTags))]
        public void Analise_withSentenceModifiers_shouldBeExpected((Token[] source, Token[] expected) pair)
        {
            var actualTokens = analyzer.Analise(pair.source)
                .Select(t => (Token)t).ToArray();

            actualTokens.Should().Equal(pair.expected);
        }

        [TestCaseSource(nameof(TokensSentencesWithDoubleTags))]
        public void Analise_withSentenceDoubleTags_shouldBeExpected((Token[] source, AnalyzedToken[] expcted) pair)
        {
            var actualTokens = analyzer.Analise(pair.source);

            actualTokens.Should().Equal(pair.expcted);
        }

        [TestCaseSource(nameof(TokensWithTagsInsideWords))]
        public void Analise_withTagsInsideWords_shouldBeExpected((Token[] source, AnalyzedToken[] expcted) pair)
        {
            var actualTokens = analyzer.Analise(pair.source);

            actualTokens.Should().Equal(pair.expcted);
        }

        [TestCaseSource(nameof(TokensWithTagsNearDigits))]
        public void Analise_withTagsNearDigits_shouldBeExpected((Token[] source, AnalyzedToken[] expcted) pair)
        {
            var actualTokens = analyzer.Analise(pair.source);

            actualTokens.Should().Equal(pair.expcted);
        }

        [TestCaseSource(nameof(TokensWithIncorrectWhiteSpaces))]
        public void Analise_whenHasIncorrectWhiteSpaces_shouldBeExpected((Token[] source, AnalyzedToken[] expcted) pair)
        {
            var actualTokens = analyzer.Analise(pair.source);

            actualTokens.Should().Equal(pair.expcted);
        }
    }
}