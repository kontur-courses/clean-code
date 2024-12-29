using FluentAssertions;
using Markdown;
using Markdown.Tags;

namespace MarkdownTests
{
    public class MdRulesForNestedTagsTests
    {
        [Test]
        public void InsideBold_ItalicShouldWork()
        {
            var result = Tag.IsNestedTagWorks(MdTagType.Bold, MdTagType.Italic);

            result.Should().BeTrue();
        }
        [Test]
        public void InsideItalic_BoldShouldNotWork()
        {
            var result = Tag.IsNestedTagWorks(MdTagType.Italic, MdTagType.Bold);

            result.Should().BeFalse();
        }

        [Test]
        public void InsideHeader_AllTagWorks()
        {
            var result = new [] {
                Tag.IsNestedTagWorks(MdTagType.Header, MdTagType.Bold),
                Tag.IsNestedTagWorks(MdTagType.Header, MdTagType.Italic),
                Tag.IsNestedTagWorks(MdTagType.Header, MdTagType.Escape)};

            result.Should().AllBeEquivalentTo(true);
        }
    }
}
