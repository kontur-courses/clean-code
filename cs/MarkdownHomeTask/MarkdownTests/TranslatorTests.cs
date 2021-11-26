using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class TranslatorTests
    {
        [Test]
        public void Translate_header_shouldBeExpected()
        {
            var tags = new[]
            {
                new AnalyzedToken(0, "#", TokenType.Tag, AnalyzedTokenType.SentenceModifer),
                new AnalyzedToken(1, "aboba", TokenType.Text)
            };
            var expected = "<h1>aboba</h1>";

            var actual = Translator.Translate(tags);

            actual.Should().Be(expected);
        }

        [Test]
        public void Translate_bolder_shouldBeExpected()
        {
            var tags = new[]
            {
                new AnalyzedToken(0, "__", TokenType.Tag, AnalyzedTokenType.Opener),
                new AnalyzedToken(1, "aboba", TokenType.Text),
                new AnalyzedToken(6, "__", TokenType.Tag, AnalyzedTokenType.Closing),
            };
            var expected = "<strong>aboba</strong>";

            var actual = Translator.Translate(tags);

            actual.Should().Be(expected);
        }

        [Test]
        public void Translate_italic_shouldBeExpected()
        {
            var tags = new[]
            {
                new AnalyzedToken(0, "_", TokenType.Tag, AnalyzedTokenType.Opener),
                new AnalyzedToken(1, "aboba", TokenType.Text),
                new AnalyzedToken(6, "_", TokenType.Tag, AnalyzedTokenType.Closing),
            };
            var expected = "<em>aboba</em>";

            var actual = Translator.Translate(tags);

            actual.Should().Be(expected);
        }

        [Test]
        public void Translate_allCases_shouldBeExpected()
        {
            var tags = new[]
            {
                new AnalyzedToken(0, "#", TokenType.Tag, AnalyzedTokenType.SentenceModifer),
                new AnalyzedToken(1, "__", TokenType.Tag, AnalyzedTokenType.Opener),
                new AnalyzedToken(3, "ti", TokenType.Text),
                new AnalyzedToken(5, " ", TokenType.WhiteSpace),
                new AnalyzedToken(6, "_", TokenType.Tag, AnalyzedTokenType.Opener),
                new AnalyzedToken(7, "aboba", TokenType.Text),
                new AnalyzedToken(13, "_", TokenType.Tag, AnalyzedTokenType.Closing),
                new AnalyzedToken(14, " ", TokenType.WhiteSpace),
                new AnalyzedToken(15, "lol", TokenType.Text),
                new AnalyzedToken(18, "__", TokenType.Tag, AnalyzedTokenType.Closing)
            };

            var expected = "<h1><strong>ti <em>aboba</em> lol</strong></h1>";

            var actual = Translator.Translate(tags);

            actual.Should().Be(expected);
        }
    }
}