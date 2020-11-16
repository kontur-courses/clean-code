using FluentAssertions;
using Markdown.Tags;
using Markdown.Tags.ItalicTag;
using NUnit.Framework;

namespace Markdown.MarkdownParserTests
{
    public class ReadAllItalicTagsTest
    {
        [Test]
        public void ReadAllItalicTags_ShouldReturnEmptyArray_OnSimpleString()
        {
            MarkdownParser.ParseAllItalicTags("asdf").Should().BeEmpty();
        }

        [Test]
        public void ReadAllItalicTags_ShouldReturnArrayWithTwoCorrectTags_OnStringWithOneTag()
        {
            var result = MarkdownParser.ParseAllItalicTags("abc _asdf_ abc");
            result.Length.Should().Be(2);
            result[0].Should().BeEquivalentTo(new OpenItalicTag(4));
            result[1].Should().BeEquivalentTo(new CloseItalicTag(9));
        }

        [Test]
        public void ReadAllItalicTags_ShouldReturnCorrectAnswer_OnStringWithManyTags()
        {
            var result = MarkdownParser.ParseAllItalicTags("_asdf_ _asf_ _asdf_");
            result.Length.Should().Be(6);
            result[0].Should().BeEquivalentTo(new OpenItalicTag(0));
            result[1].Should().BeEquivalentTo(new CloseItalicTag(5));
            result[2].Should().BeEquivalentTo(new OpenItalicTag(7));
            result[3].Should().BeEquivalentTo(new CloseItalicTag(11));
            result[4].Should().BeEquivalentTo(new OpenItalicTag(13));
            result[5].Should().BeEquivalentTo(new CloseItalicTag(18));
        }
    }
}