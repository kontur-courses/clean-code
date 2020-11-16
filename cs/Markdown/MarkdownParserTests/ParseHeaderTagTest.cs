using FluentAssertions;
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
            MarkdownParser.ParseHeaderTag("asdf").Should().BeEmpty();
        }

        [Test]
        public void ReadHeaderToken_ShouldReturnCorrectArray_OnHeaderSting()
        {
            var result = MarkdownParser.ParseHeaderTag("#asdf");
            result.Length.Should().Be(2);
            result[0].Should().BeEquivalentTo(new OpenHeaderTag(0));
            result[1].Should().BeEquivalentTo(new CloseHeaderTag(5));
        }
        
    }
}