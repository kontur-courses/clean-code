using FluentAssertions;
using Markdown;
using Markdown.Tags;
using NUnit.Framework;

namespace Markdown_Tests
{
    public class ParsedText_Should
    {
        [TestCaseSource(typeof(ParsedTextData), nameof(ParsedTextData.NullArguments))]
        public void Throw_WhenNullArguments(string paragraph, List<ITag> tags)
        {
            var action = new Action(() => new ParsedText(paragraph, tags));
            action.Should().Throw<ArgumentException>().Which.Message.Contains("null");
        }

        [TestCaseSource(typeof(ParsedTextData), nameof(ParsedTextData.TagBeyondParagraph))]
        public void Throw_WhenTagPosition_OutsideParagraph(string paragraph, List<ITag> tags)
        {
            var action = new Action(() => new ParsedText(paragraph, tags));
            action.Should().Throw<ArgumentException>().Which.Message.Contains("beyond paragraph");
        }

        [TestCaseSource(typeof(ParsedTextData), nameof(ParsedTextData.TagNotOrderedByAscending))]
        public void Throw_WhenTagPosition_IsNotOrderedByAscending(string paragraph, List<ITag> tags)
        {
            var action = new Action(() => new ParsedText(paragraph, tags));
            action.Should().Throw<ArgumentException>().Which.Message.Contains("should be ordered");
        }
    }
}
