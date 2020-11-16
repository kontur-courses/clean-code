using FluentAssertions;
using Markdown.Tags;
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
            result.Length.Should().Be(1);
            result[0].Should().BeEquivalentTo(new OpenUnOrderedListTag(0));
            
            result = MarkdownParser.ParseUnOrderedListTag("asdf");
            result.Length.Should().Be(1);
            result[0].Should().BeEquivalentTo(new CloseUnOrderedListTag(0));
        }
    }
}