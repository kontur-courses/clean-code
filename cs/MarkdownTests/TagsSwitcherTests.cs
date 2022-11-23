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

            result.Should().Be($@"a<p><img src=""bb"" alt=""AA""></p>a");
        }
    }
}