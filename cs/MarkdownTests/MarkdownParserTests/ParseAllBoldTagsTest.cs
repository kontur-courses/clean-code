using FluentAssertions;
using Markdown.Parsers;
using Markdown.Tags.BoldTag;
using NUnit.Framework;

namespace MarkdownTests.MarkdownParserTests
{
    public class ParseAllBoldTagsTest
    {
        [Test]
        public void ParseAllBoldTags_ShouldReturnEmptyArray_OnSimpleString()
        {
            BoldParser.ParseTags("asdf").Should().BeEmpty();
        }

        [Test]
        public void ParseAllBoldTags_ShouldReturnTwoTags_OnStringWithOneTag()
        {
            var result = BoldParser.ParseTags("__asdf__");
            result.Should().HaveCount(2);
            result[0].Should().BeEquivalentTo(new OpenBoldTag(0));
            result[1].Should().BeEquivalentTo(new CloseBoldTag(6));
        }

        [Test]
        public void ParseAllBoldTags_ShouldReturnEmptyArray_OnShieldedOpenTag()
        {
            var result = BoldParser.ParseTags(@"\__asdf__");
            result.Should().BeEmpty();
        }
        
        [Test]
        public void ParseAllBoldTags_ShouldReturnEmptyArray_OnShieldedCloseTag()
        {
            var result = BoldParser.ParseTags(@"__asdf\__");
            result.Should().BeEmpty();
        }
    }
}