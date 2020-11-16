using FluentAssertions;
using Markdown.Tags;
using Markdown.Tags.ListIemTag;
using Markdown.Tags.UnorderedListTag;
using NUnit.Framework;

namespace Markdown.MarkdownParserTests
{
    public class ParseUnOrderedListTagTests
    {
        [Test]
        public void ParseUnorderedList_ShouldReturnEmpty_ArrayOnSimpleText()
        {
            MarkdownParser.ParseUnOrderedListTag("asdf").Should().BeEmpty();
        }

        [Test]
        public void ParseUnorderedList_ShouldReturnTwoCorrectTokens()
        {
            var result = MarkdownParser.ParseUnOrderedListTag("* asdf");
            result.Length.Should().Be(3);
            result[0].Should().BeEquivalentTo(new OpenUnOrderedListTag(-1));
            result[1].Should().BeEquivalentTo(new OpenListItemTag(0));
            result[2].Should().BeEquivalentTo(new CloseListItemTag(6));
            
            result = MarkdownParser.ParseUnOrderedListTag("asdf");
            result.Length.Should().Be(1);
            result[0].Should().BeEquivalentTo(new CloseUnOrderedListTag(-1));
        }
    }
}