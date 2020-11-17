using FluentAssertions;
using Markdown.Parsers;
using Markdown.Tags.ListIemTag;
using Markdown.Tags.UnorderedListTag;
using NUnit.Framework;

namespace MarkdownTests.MarkdownParserTests
{
    public class ParseUnOrderedListTagTests
    {
        [Test]
        public void ParseUnorderedList_ShouldReturnEmpty_ArrayOnSimpleText()
        {
            UnOrderedListParser.ParseTags("asdf").Should().BeEmpty();
        }

        [Test]
        public void ParseUnorderedList_ShouldReturnTwoCorrectTokens()
        {
            var result = UnOrderedListParser.ParseTags("* asdf");
            result.Should().HaveCount(3);
            result[0].Should().BeEquivalentTo(new OpenUnOrderedListTag());
            result[1].Should().BeEquivalentTo(new OpenListItemTag(0));
            result[2].Should().BeEquivalentTo(new CloseListItemTag(6));
            
            result = UnOrderedListParser.ParseTags("asdf");
            result.Should().HaveCount(1);
            result[0].Should().BeEquivalentTo(new CloseUnOrderedListTag());
        }
    }
}