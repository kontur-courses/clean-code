using FluentAssertions;
using MarkdownTask.MarkdownParsers;
using NUnit.Framework;
using static MarkdownTask.TagInfo;

namespace MarkdownTask.MarkdownTests
{
    [TestFixture]
    public class LinkTagParserTests
    {
        [Test]
        public void Process_LinkTags_ReturnsCorrectTokens()
        {
            var inputText = "a [b](c) d";

            var expected = new List<Token>
            {
                new Token(TagType.Link, 2, Tag.Open, 6),
                new Token(TagType.Link, 7, Tag.Close, 0)
            };

            var result = new LinkTagParser().Parse(inputText).ToList();

            result.Count.Should().Be(expected.Count);
            result[0].Should().BeEquivalentTo(expected[0]);
            result[1].Should().BeEquivalentTo(expected[1]);
        }
    }
}
