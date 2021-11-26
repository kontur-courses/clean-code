using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;
using Sentence = System.Collections.Generic.IReadOnlyList<Markdown.Token>;

namespace MarkdownTests
{
    [TestFixture]
    public class TokenizerTests
    {
        [Test]
        public void Tokenize_withSimpleTag_shouldReturnExpectedTokens()
        {
            var text = "#abc";
            var expectedTokens = new[]
            {
                new Token(0, "#", TokenType.Tag),
                new Token(1, "abc", TokenType.Text),
            };

            var tokens = new Tokenizer().Tokenize(text, new[] { "#" });

            tokens.First().Should().Equal(expectedTokens);
        }

        [Test]
        public void Tokenize_withCompoundTag_shouldReturnExpectedTokens()
        {
            var text = "__abc";
            var expectedTokens = new[]
            {
                new[]
                {
                    new Token(0, "__", TokenType.Tag),
                    new Token(2, "abc", TokenType.Text),
                }
            };

            var actualTokens = new Tokenizer().Tokenize(text, new[] { "_", "__" });

            TokenSetsIsEqual(actualTokens, expectedTokens);
        }

        [Test]
        public void Tokenize_whenTextHasPrefixOfTag_shouldReturnExpectedTokens()
        {
            var text = "__abc43";
            var expectedTokens = new[]
            {
                new[] { new Token(0, "__abc43", TokenType.Text), }
            };

            var actualTokens = new Tokenizer().Tokenize(text, new[] { "___" });

            TokenSetsIsEqual(actualTokens, expectedTokens);
        }

        [Test]
        public void Tokenize_whenTextHasPrefixOfTag_shouldReturnExpectedTokens()
        {
            var text = "__abc43";
            var tokens = new Tokenizer().Tokenize(text, new[] {"___"});
            var expectedTokens = new[]
            {
                new Token(0, "__abc", TokenType.Text),
                new Token(5, "43", TokenType.Number)
            };

            tokens.Should().Equal(expectedTokens);
        }
        
        [Test]
        public void Tokenize_withCompoundAndSimpleTags_shouldReturnExpectedTokens()
        {
            var text = "__abc#qwer";
            var expectedTokens = new[]
            {
                new[]
                {
                    new Token(0, "__", TokenType.Tag),
                    new Token(2, "abc", TokenType.Text),
                    new Token(5, "#", TokenType.Tag),
                    new Token(6, "qwer", TokenType.Text)
                }
            };

            var actualTokens = new Tokenizer().Tokenize(text, new[] { "_", "__", "#" });

            TokenSetsIsEqual(actualTokens, expectedTokens);
        }

        [Test]
        public void Tokenize_withEscape_shouldReturnExpectedTokens()
        {
            var text = @"\__abc";
            var expectedTokens = new[]
            {
                new[]
                {
                    new Token(0, "_", TokenType.Text),
                    new Token(2, "_", TokenType.Tag),
                    new Token(3, "abc", TokenType.Text)
                }
            };

            var actualTokens = new Tokenizer().Tokenize(text, new[] { "_", "__", "#" });

            TokenSetsIsEqual(actualTokens, expectedTokens);
        }

        [Test]
        public void Tokenize_withDoubleEscape_shouldReturnExpectedTokens()
        {
            var text = @"_abc\\qwer";
            var expectedTokens = new[]
            {
                new[]
                {
                    new Token(0, "_", TokenType.Tag),
                    new Token(1, @"abc\qwer", TokenType.Text)
                }
            };

            var actualTokens = new Tokenizer().Tokenize(text, new[] { "_", "__", "#" });

            TokenSetsIsEqual(actualTokens, expectedTokens);
        }

        [Test]
        public void Tokenize_withWhiteSpace_shouldReturnExpectedTokens()
        {
            var text = @"_ abc\\ qwer";
            var expectedTokens = new[]
            {
                new[]
                {
                    new Token(0, "_", TokenType.Tag),
                    new Token(1, " ", TokenType.WhiteSpace),
                    new Token(2, @"abc\", TokenType.Text),
                    new Token(7, " ", TokenType.WhiteSpace),
                    new Token(8, "qwer", TokenType.Text)
                }
            };

            var actualTokens = new Tokenizer().Tokenize(text, new[] { "_", "__", "#" });

            TokenSetsIsEqual(actualTokens, expectedTokens);
        }

        [Test]
        public void Tokenize_withWhiteSpace_shouldReturnExpectedTokens2()
        {
            var text = @"a a a a";
            var expectedTokens = new[]
            {
                new[]
                {
                    new Token(0, "a", TokenType.Text),
                    new Token(1, " ", TokenType.WhiteSpace),
                    new Token(2, "a", TokenType.Text),
                    new Token(3, " ", TokenType.WhiteSpace),
                    new Token(4, "a", TokenType.Text),
                    new Token(5, " ", TokenType.WhiteSpace),
                    new Token(6, "a", TokenType.Text)
                }
            };

            var actualTokens = new Tokenizer().Tokenize(text, new[] { "_", "__", "#" });

            TokenSetsIsEqual(actualTokens, expectedTokens);
        }

        [Test]
        public void Tokenize_withAllCases_shouldReturnExpectedTokens()
        {
            var text = @"_ __ab1c\\ q234wer";
            var actualTokens = new Tokenizer().Tokenize(text, new[] { "_", "__", "#" });

            var expectedTokens = new[]
            {
                new[]
                {
                    new Token(0, "_", TokenType.Tag),
                    new Token(1, " ", TokenType.WhiteSpace),
                    new Token(2, "__", TokenType.Tag),
                    new Token(4, @"ab1c\", TokenType.Text),
                    new Token(10, " ", TokenType.WhiteSpace),
                    new Token(11, "q234wer", TokenType.Text)
                }
            };

            TokenSetsIsEqual(expectedTokens, actualTokens);
        }

        private static void TokenSetsIsEqual(IEnumerable<Sentence> actualTokens, IEnumerable<Sentence> expectedTokens)
        {
            foreach (var pair in actualTokens
                .Zip(expectedTokens, (x, y) => (x, y)))
            {
                pair.x.Should().Equal(pair.y);
            }
        }
    }
}