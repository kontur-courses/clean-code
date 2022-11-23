using FluentAssertions;
using Markdown.MarkerLogic;
using Markdown.TagClasses;
using Markdown.TagClasses.ITagInterfaces;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class TagsFilterTests
    {
        private readonly TagsFilter sut = new();

        [Test]
        public void FilterTags_AddSinglePairedTag_ShouldFilterIt()
        {
            var text = "_aaa";
            List<ITag> tags = new()
            {
                new PairedTag(0, TagType.Emphasis, true)
            };

            var result = sut.FilterTags(tags, text);

            result.Count.Should().Be(0);
        }

        [Test]
        [Description("Preventing intersection")]
        public void FilterTags_AddTagsWIthInconclusivePositions_ShouldSwitchIt()
        {
            var text = "_aa___aa__";
            List<ITag> tags = new()
            {
                new PairedTag(0, TagType.Emphasis, true),
                new PairedTag(3, TagType.Strong, true, true),
                new PairedTag(5, TagType.Emphasis, true, true),
                new PairedTag(8, TagType.Strong, false, true)
            };

            List<ITag> expectedResult = new()
            {
                new PairedTag(0, TagType.Emphasis, true),
                new PairedTag(3, TagType.Emphasis, false, true),
                new PairedTag(4, TagType.Strong, true),
                new PairedTag(8, TagType.Strong, false, true)
            };
            var result = sut.FilterTags(tags, text);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        [Description("Preventing intersection")]
        public void FilterTags_AddTagsWIthInconclusivePositions_ShouldNotSwitchIt()
        {
            var text = "__aa___aa_";
            List<ITag> tags = new()
            {
                new PairedTag(0, TagType.Strong, true),
                new PairedTag(4, TagType.Strong, true, true),
                new PairedTag(6, TagType.Emphasis, true, true),
                new PairedTag(9, TagType.Emphasis, false, true)
            };

            List<ITag> expectedResult = new()
            {
                new PairedTag(0, TagType.Strong, true),
                new PairedTag(4, TagType.Strong, true, true),
                new PairedTag(6, TagType.Emphasis, true, true),
                new PairedTag(9, TagType.Emphasis, false, true)
            };
            var result = sut.FilterTags(tags, text);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void FilterTags_AddTagInTheMiddle_ShouldSpecifyItRoleInPair()
        {
            var text = "_aa_a";
            List<ITag> tags = new()
            {
                new PairedTag(0, TagType.Emphasis, true),
                new PairedTag(4, TagType.Emphasis, true, true),
            };

            List<ITag> expectedResult = new()
            {
                new PairedTag(0, TagType.Emphasis, true),
                new PairedTag(4, TagType.Emphasis, false, true),
            };
            var result = sut.FilterTags(tags, text);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void FilterTags_AddIntersectedTags_ShouldFilterIt()
        {
            var text = "_a __a_ a__";
            List<ITag> tags = new()
            {
                new PairedTag(0, TagType.Emphasis, true),
                new PairedTag(3, TagType.Strong, true),
                new PairedTag(6, TagType.Emphasis, false, true),
                new PairedTag(9, TagType.Strong, false, true)
            };

            var result = sut.FilterTags(tags, text);

            result.Count.Should().Be(0);
        }

        [Test]
        public void FilterTags_AddStrongInsideEmphasis_ShouldFilterOutStrong()
        {
            var text = "_a __a__ a_";
            List<ITag> tags = new()
            {
                new PairedTag(0, TagType.Emphasis, true),
                new PairedTag(3, TagType.Strong, true),
                new PairedTag(6, TagType.Strong, false, true),
                new PairedTag(10, TagType.Emphasis, false, true)
            };
            List<ITag> expectedResult = new()
            {
                new PairedTag(0, TagType.Emphasis, true),
                new PairedTag(10, TagType.Emphasis, false, true)
            };

            var result = sut.FilterTags(tags, text);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}