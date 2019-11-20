using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tokenization.Tests
{
    [TestFixture]
    public class TokenReaderTests
    {
        private ITokenReaderConfiguration configuration;
        private TokenReader tokenReader;

        [SetUp]
        public void SetUp()
        {
            configuration = new TestTokenReaderConfiguration();
            tokenReader = new TokenReader(configuration);
        }


        [Test]
        public void ReadAllTokens_ShouldReturnCorrect_WhenSeparatorInMiddle()
        {
            var text = "sample text";

            var tokens = tokenReader.ReadAllTokens(text);

            tokens.ShouldBeEquivalentTo(new[]
            {
                new Token(0, "sample", false),
                new Token(6, " ", true),
                new Token(7, "text", false),
            });
        }

        [Test]
        public void ReadAllTokens_ShouldReturnCorrect_WhenSeparatorInBeginning()
        {
            var text = " sample text";

            var tokens = tokenReader.ReadAllTokens(text);

            tokens.ShouldBeEquivalentTo(new[]
            {
                new Token(0, " ", true),
                new Token(1, "sample", false),
                new Token(7, " ", true),
                new Token(8, "text", false),
            });
        }

        [Test]
        public void ReadAllTokens_ShouldReturnCorrect_WhenSeparatorInEnd()
        {
            var text = "sample text ";

            var tokens = tokenReader.ReadAllTokens(text);

            tokens.ShouldBeEquivalentTo(new[]
            {
                new Token(0, "sample", false),
                new Token(6, " ", true),
                new Token(7, "text", false),
                new Token(11, " ", true),
            });
        }
    }

    public class TestTokenReaderConfiguration : ITokenReaderConfiguration
    {
        public bool IsSeparator(string text, int position)
        {
            return char.IsWhiteSpace(text[position]);
        }

        public int GetSeparatorLength(string text, int position)
        {
            return 1;
        }

        public string GetSeparatorValue(string text, int position)
        {
            return " ";
        }
    }
}