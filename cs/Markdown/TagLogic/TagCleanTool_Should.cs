using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    [TestFixture]
    class TagCleanTool_Should
    {
        [Test]
        public void GetCorrectMarkdownTags_Should_RemoveShieldedDelimiters()
        {
            var inputString = @"\_a\_";
            var tags = new Dictionary<string, List<Tag>>
            {
                ["_"] = new List<Tag>
                {
                    new Tag("_", 1, TagType.Opening, Markdown.MarkDownTagsPriority["_"]),
                    new Tag("_", 3, TagType.Closing, Markdown.MarkDownTagsPriority["_"]),
                }
            };

            var correctTags = TagCleanTool.GetCorrectMarkdownTags(inputString, tags);

            correctTags.Count.Should().Be(0);
        }

        [Test]
        public void GetCorrectMarkdownTags_Should_RemoveAllUnclosedSeparators()
        {
            var inputString = @"_a_ _a";
            var tags = new Dictionary<string, List<Tag>>
            {
                ["_"] = new List<Tag>
                {
                    new Tag("_", 0, TagType.Opening, Markdown.MarkDownTagsPriority["_"]),
                    new Tag("_", 2, TagType.Closing, Markdown.MarkDownTagsPriority["_"]),
                    new Tag("_", 4, TagType.Opening, Markdown.MarkDownTagsPriority["_"]),
                }
            };

            var correctTags = TagCleanTool.GetCorrectMarkdownTags(inputString, tags);

            correctTags.Count.Should().Be(2);
            correctTags.First().Should().BeEquivalentTo(new Tag("_", 0, TagType.Opening, Markdown.MarkDownTagsPriority["_"]));
            correctTags.Last().Should().BeEquivalentTo(new Tag("_", 2, TagType.Closing, Markdown.MarkDownTagsPriority["_"]));
        }

        [Test]
        public void GetCorrectMarkdownTags_Should_RemoveIncorrectNestingTags()
        {
            var inputString = "_a __aa__ a_";
            var tags = new Dictionary<string, List<Tag>>
            {
                ["_"] = new List<Tag>
                {
                    new Tag("_", 0, TagType.Opening, Markdown.MarkDownTagsPriority["_"]),
                    new Tag("__", 3, TagType.Opening, Markdown.MarkDownTagsPriority["__"]),
                    new Tag("__", 7, TagType.Closing, Markdown.MarkDownTagsPriority["__"]),
                    new Tag("_", 11, TagType.Closing, Markdown.MarkDownTagsPriority["_"]),
                }
            };

            var correctTags = TagCleanTool.GetCorrectMarkdownTags(inputString, tags);

            correctTags.Count.Should().Be(2);
            correctTags.First().Should().BeEquivalentTo(new Tag("_", 0, TagType.Opening, Markdown.MarkDownTagsPriority["_"]));
            correctTags.Last().Should().BeEquivalentTo(new Tag("_", 11, TagType.Closing, Markdown.MarkDownTagsPriority["_"]));
        }
    }
}
