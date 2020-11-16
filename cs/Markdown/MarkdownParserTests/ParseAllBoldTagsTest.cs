using FluentAssertions;
using Markdown.Tags;
using Markdown.Tags.BoldTag;
using NUnit.Framework;

namespace Markdown.MarkdownParserTests
{
    public class ReadAllBoldTagsTest
    {
        [Test]
        public void ReadAllBoldTags_ShouldReturnEmptyArray_OnSimpleString()
        {
            MarkdownParser.ParseAllBoldTags("asdf").Should().BeEmpty();
        }

        [Test]
        public void ReadAllBoldTags_ShouldReturnTwoTags_OnStringWithOneTag()
        {
            var result = MarkdownParser.ParseAllBoldTags("__asdf__");
            result.Length.Should().Be(2);
            result[0].Should().BeEquivalentTo(new OpenBoldTag(0));
            result[1].Should().BeEquivalentTo(new CloseBoldTag(6));
        }

        [Test]
        public void ReadAllBoldTags_ShouldReturnEmptyArray_OnShieldedOpenTag()
        {
            var result = MarkdownParser.ParseAllBoldTags(@"\__asdf__");
            result.Length.Should().Be(0);
        }
        
        [Test]
        public void ReadAllBoldTags_ShouldReturnEmptyArray_OnShieldedCloseTag()
        {
            var result = MarkdownParser.ParseAllBoldTags(@"__asdf\__");
            result.Length.Should().Be(0);
        }
    }
}