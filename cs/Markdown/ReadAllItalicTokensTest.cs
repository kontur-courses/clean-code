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
            result[0].Should().BeEquivalentTo(new Tag("<em>", 4));
            result[1].Should().BeEquivalentTo(new Tag("</em>", 9));
        }

        [Test]
        public void ReadAllItalicTokens_ShouldReturnCorrectAnswer_OnStringWithManyTokens()
        {
            var result = MarkdownParser.ReadAllItalicTokens("_asdf_ _asf_ _asdf_");
            result.Length.Should().Be(6);
            result[0].Should().BeEquivalentTo(new Tag("<em>", 0));
            result[1].Should().BeEquivalentTo(new Tag("</em>", 5));
            result[2].Should().BeEquivalentTo(new Tag("<em>", 7));
            result[3].Should().BeEquivalentTo(new Tag("</em>", 11));
            result[4].Should().BeEquivalentTo(new Tag("<em>", 13));
            result[5].Should().BeEquivalentTo(new Tag("</em>", 18));
        }
    }
}