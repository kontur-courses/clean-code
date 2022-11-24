using FluentAssertions;
using Markdown.MarkerLogic;
using Markdown.TagClasses;
using Markdown.TagClasses.ITagInterfaces;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class TagsSwitcherTests
    {
        private readonly TagsSwitcher sut = new();

        [Test]
        public void SwitchTags_AddHeaderTag_ShouldSwitchIt()
        {
            var text = "#aa";
            var tags = new List<ITag>
            {
                new HeaderTag(0, TagType.Header)
            };

            var result = sut.SwitchTags(tags, text);

            result.Should().Be("<h1>aa</h1>");
        }

        [Test]
        public void SwitchTags_AddStrongTagsPair_ShouldSwitchThem()
        {
            var text = "__aa__";
            var tags = new List<ITag>
            {
                new PairedTag(0, TagType.Strong, true),
                new PairedTag(4, TagType.Strong, false, true)
            };

            var result = sut.SwitchTags(tags, text);

            result.Should().Be("<strong>aa</strong>");
        }

        [Test]
        public void SwitchTags_AddEmphasisTagPair_ShouldSwitchThem()
        {
            var text = "_aa_";
            var tags = new List<ITag>
            {
                new PairedTag(0, TagType.Emphasis, true),
                new PairedTag(3, TagType.Emphasis, false, true)
            };

            var result = sut.SwitchTags(tags, text);

            result.Should().Be("<em>aa</em>");
        }

        [Test]
        public void SwitchTags_AddPictureTag_ShouldSwitchIt()
        {
            var text = "a![AA](bb)a";
            var tags = new List<ITag>
            {
                new TextTag(1, TagType.Picture, 9, false, "bb", "AA"),
            };

            var result = sut.SwitchTags(tags, text);

            result.Should().Be($@"a<img src=""bb"" alt=""AA"">a");
        }

        [Test]
        public void SwitchTags_AddEscapedPictureTag_ShouldOnlyRemoveEscapeSymbol()
        {
            var text = @"\![AA](bb)a";
            var tags = new List<ITag>
            {
                new TextTag(1, TagType.Picture, 9, true, "bb", "AA"),
            };

            var result = sut.SwitchTags(tags, text);

            result.Should().Be(@"![AA](bb)a");
        }

        [Test]
        public void SwitchTags_AddManyTags_ShouldCorrectlyAdjustOtherTagsPositions()
        {
            var text = @"#_aa_ __bb__ _cc_";
            var tags = new List<ITag>
            {
                new HeaderTag(0, TagType.Header),
                new PairedTag(1, TagType.Emphasis, true),
                new PairedTag(4, TagType.Emphasis, false, true),
                new PairedTag(6, TagType.Strong, true),
                new PairedTag(10, TagType.Strong, false, true),
                new PairedTag(13, TagType.Emphasis, true),
                new PairedTag(16, TagType.Emphasis, false, true),
            };

            var result = sut.SwitchTags(tags, text);

            result.Should().Be(@"<h1><em>aa</em> <strong>bb</strong> <em>cc</em></h1>");
        }

        [Test]
        public void SwitchTags_RemovingEscapesFromEscapedTags_ShouldCorrectlyAdjustOtherTagsPositions()
        {
            var text = @"#_aa_ \__bb\__ _cc_";
            var tags = new List<ITag>
            {
                new HeaderTag(0, TagType.Header),
                new PairedTag(1, TagType.Emphasis, true),
                new PairedTag(4, TagType.Emphasis, false, true),
                new PairedTag(7, TagType.Strong, isEscaped: true),
                new PairedTag(12, TagType.Strong, isEscaped: true),
                new PairedTag(15, TagType.Emphasis, true),
                new PairedTag(18, TagType.Emphasis, false, true),
            };

            var result = sut.SwitchTags(tags, text);

            result.Should().Be(@"<h1><em>aa</em> __bb__ <em>cc</em></h1>");
        }

        [Test]
        public void SwitchTags_AddEscapedSlashes_ShouldRemoveEscapes()
        {
            var text = @"#\\_aa_ \__bb\__ \\_cc_";
            var tags = new List<ITag>
            {
                new HeaderTag(0, TagType.Header),
                new PairedTag(3, TagType.Emphasis, true),
                new PairedTag(6, TagType.Emphasis, false, true),
                new PairedTag(9, TagType.Strong, isEscaped: true),
                new PairedTag(14, TagType.Strong, isEscaped: true),
                new PairedTag(19, TagType.Emphasis, true),
                new PairedTag(22, TagType.Emphasis, false, true),
            };

            var result = sut.SwitchTags(tags, text);

            result.Should().Be(@"<h1>\<em>aa</em> __bb__ \<em>cc</em></h1>");
        }
    }
}