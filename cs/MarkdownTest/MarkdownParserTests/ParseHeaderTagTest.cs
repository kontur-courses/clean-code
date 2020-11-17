using FluentAssertions;
using Markdown.Parsers;
using Markdown.Tags;
using Markdown.Tags.HeaderTag;
using NUnit.Framework;

namespace Markdown.MarkdownParserTests
{
    public class ReadHeaderTag
    {
        [Test]
        public void ReadHeaderToken_ShouldReturnEmptyArray_OnSimpleString()
        {
            HeaderParser.ParseTags("asdf").Should().BeEmpty();
        }

        [Test]
        public void ReadHeaderToken_ShouldReturnCorrectArray_OnHeaderSting()
        {
            var result = HeaderParser.ParseTags("#asdf");
            result.Length.Should().Be(2);
            result[0].Should().BeEquivalentTo(new OpenHeaderTag(0));
            result[1].Should().BeEquivalentTo(new CloseHeaderTag(5));
        }
        
    }
}