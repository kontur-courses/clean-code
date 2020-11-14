using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class ReadAllItalicTokensTest
    {
        [Test]
        public void ReadAllItalicTokens_ShouldReturnEmptyArray_OnSimpleString()
        {
            MarkdownParser.ReadAllItalicTokens("asdf").Should().BeEmpty();
        }

        [Test]
        public void ReadAllItalicTokens_ShouldReturnArrayWithTwoCorrectTags_OnStringWithOneToken()
        {
            var result = MarkdownParser.ReadAllItalicTokens("abc _asdf_ abc");
            result.Length.Should().Be(2);
            result[0].Should().BeEquivalentTo(new Tag("<em>", 4, 1));
            result[1].Should().BeEquivalentTo(new Tag("</em>", 9, 1));
        }

        [Test]
        public void ReadAllItalicTokens_ShouldReturnCorrectAnswer_OnStringWithManyTokens()
        {
            var result = MarkdownParser.ReadAllItalicTokens("_asdf_ _asf_ _asdf_");
            result.Length.Should().Be(6);
            result[0].Should().BeEquivalentTo(new Tag("<em>", 0, 1));
            result[1].Should().BeEquivalentTo(new Tag("</em>", 5, 1));
            result[2].Should().BeEquivalentTo(new Tag("<em>", 7, 1));
            result[3].Should().BeEquivalentTo(new Tag("</em>", 11, 1));
            result[4].Should().BeEquivalentTo(new Tag("<em>", 13, 1));
            result[5].Should().BeEquivalentTo(new Tag("</em>", 18, 1));
        }
    }
}