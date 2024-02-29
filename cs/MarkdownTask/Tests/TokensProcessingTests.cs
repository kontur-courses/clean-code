using FluentAssertions;
using NUnit.Framework;

namespace MarkdownTask.MarkdownTests
{
    public class TokensProcessingTests
    {
        [Test]
        public void Process_NoTokens_ReturnsEmptyList()
        {
            var tokens = new List<Token>();

            var processedTokens = TokensProcessing.Process(tokens);

            processedTokens.Should().BeEmpty();
        }

        [Test]
        public void Process_OphranedToken_ReturnsNoToken()
        {
            var tokens = new List<Token>
            {
                new Token(TagInfo.TagType.Italic, 1,TagInfo.Tag.Open, 1),
                new Token(TagInfo.TagType.Strong, 5,TagInfo.Tag.Close, 2),
            };

            var processedTokens = TokensProcessing.Process(tokens);

            processedTokens.Should().BeEmpty();
        }

        [Test]
        public void Process_ValidNesting_ReturnsProcessedTokens()
        {
            var tokens = new List<Token>
            {
                new Token(TagInfo.TagType.Strong, 1,TagInfo.Tag.Open, 2),
                new Token(TagInfo.TagType.Italic, 3,TagInfo.Tag.Open, 1),
                new Token(TagInfo.TagType.Italic, 5,TagInfo.Tag.Close, 1),
                new Token(TagInfo.TagType.Strong, 7,TagInfo.Tag.Close, 2),
            };

            var processedTokens = TokensProcessing.Process(tokens);

            processedTokens.Should().HaveCount(4);
        }

        [Test]
        public void Process_InvalidNesting_ReturnsProcessedTokens()
        {
            var tokens = new List<Token>
            {
                new Token(TagInfo.TagType.Italic, 1,TagInfo.Tag.Open, 1),
                new Token(TagInfo.TagType.Strong, 2,TagInfo.Tag.Open, 2),
                new Token(TagInfo.TagType.Strong, 3,TagInfo.Tag.Close, 2),
                new Token(TagInfo.TagType.Italic, 4,TagInfo.Tag.Close, 1),
            };

            var processedTokens = TokensProcessing.Process(tokens);

            processedTokens.Should().HaveCount(2);
        }

        [Test]
        public void Process_TagsIntersection_ReturnsNoTokens()
        {
            var tokens = new List<Token>
            {
                new Token(TagInfo.TagType.Strong, 1,TagInfo.Tag.Open, 2),
                new Token(TagInfo.TagType.Italic, 3,TagInfo.Tag.Open, 1),
                new Token(TagInfo.TagType.Strong, 5,TagInfo.Tag.Close, 2),
                new Token(TagInfo.TagType.Italic, 7,TagInfo.Tag.Close, 1),
            };

            var processedTokens = TokensProcessing.Process(tokens);

            processedTokens.Should().BeEmpty();
        }
    }
}