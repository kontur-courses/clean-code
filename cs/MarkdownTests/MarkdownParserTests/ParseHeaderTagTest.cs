using FluentAssertions;
using Markdown.Parsers;
using Markdown.Tags.HeaderTag;
using NUnit.Framework;

namespace MarkdownTests.MarkdownParserTests
{
    public class ParseHeaderTag
    {
        [Test]
        public void ParseHeaderToken_ShouldReturnEmptyArray_OnSimpleString()
        {
            HeaderParser.ParseTags("asdf").Should().BeEmpty();
        }

        [Test]
        public void ParseHeaderToken_ShouldReturnCorrectArray_OnHeaderSting()
        {
            var result = HeaderParser.ParseTags("#asdf");
            result.Should().HaveCount(2);
            result[0].Should().BeEquivalentTo(new OpenHeaderTag(0));
            result[1].Should().BeEquivalentTo(new CloseHeaderTag(5));
        }
        
    }
}