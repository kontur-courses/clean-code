using FluentAssertions;
using Markdown.Parsers;
using Markdown.Tags.ItalicTag;
using NUnit.Framework;

namespace MarkdownTests.MarkdownParserTests
{
    public class ParseAllItalicTagsTest
    {
        [Test]
        public void ParseAllItalicTags_ShouldReturnEmptyArray_OnSimpleString()
        {
            ItalicParser.ParseTags("asdf").Should().BeEmpty();
        }

        [Test]
        public void ParseAllItalicTags_ShouldReturnArrayWithTwoCorrectTags_OnStringWithOneTag()
        {
            var result = ItalicParser.ParseTags("abc _asdf_ abc");
            result.Should().HaveCount(2);
            result[0].Should().BeEquivalentTo(new OpenItalicTag(4));
            result[1].Should().BeEquivalentTo(new CloseItalicTag(9));
        }

        [Test]
        public void ParseAllItalicTags_ShouldReturnCorrectAnswer_OnStringWithManyTags()
        {
            var result = ItalicParser.ParseTags("_asdf_ _asf_ _asdf_");
            result.Should().HaveCount(6);
            result[0].Should().BeEquivalentTo(new OpenItalicTag(0));
            result[1].Should().BeEquivalentTo(new CloseItalicTag(5));
            result[2].Should().BeEquivalentTo(new OpenItalicTag(7));
            result[3].Should().BeEquivalentTo(new CloseItalicTag(11));
            result[4].Should().BeEquivalentTo(new OpenItalicTag(13));
            result[5].Should().BeEquivalentTo(new CloseItalicTag(18));
        }
    }
}