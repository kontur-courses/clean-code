using FluentAssertions;
using Markdown.Converters;
using Markdown.Tag;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class TagsConverterTests
    {
        [TestCase(null, TestName = "text is null")]
        [TestCase("", TestName = "text is empty")]
        [TestCase("    ", TestName = "text is whitespace")]
        [TestCase("lol", TestName = "text do not contains tags")]
        [TestCase("_", TestName = "text contains not closed tag")]
        public void GetAllTags_ReturnsEmptyList(string text)
        {
            TagsConverter.GetAllTags(text).Should().BeEmpty();
        }

        [Test]
        public void GetAllTags_RecognizesSingleItalicsTag()
        {
            var text = "_a_";
            var tags = TagsConverter.GetAllTags(text);

            tags.Count.Should().Be(1);
            tags.Pop().Should().BeEquivalentTo(new ItalicsTag(0, 2));
        }

        [Test]
        public void GetAllTags_RecognizesSingleTitleTag()
        {
            var text = "#a\n";
            var tags = TagsConverter.GetAllTags(text);

            tags.Count.Should().Be(1);
            tags.Pop().Should().BeEquivalentTo(new TitleTag(0, 2));
        }

        [Test]
        public void GetAllTags_RecognizesNotClosedTitleTag()
        {
            var text = "#a";
            var tags = TagsConverter.GetAllTags(text);

            tags.Count.Should().Be(1);
            tags.Pop().Should().BeEquivalentTo(new TitleTag(0, 2));
        }

        [Test]
        public void GetAllTags_RecognizesStrongTextTag()
        {
            var text = "__a__";
            var tags = TagsConverter.GetAllTags(text);

            tags.Count.Should().Be(1);
            tags.Pop().Should().BeEquivalentTo(new StrongTextTag(0, 4));
        }

        [Test]
        public void GetAllTags_RecognizesManyTagsOfTheSameType()
        {
            var text = "_a_ b _c_";
            var tags = TagsConverter.GetAllTags(text);

            tags.Count.Should().Be(2);
            tags.Pop().Should().BeEquivalentTo(new ItalicsTag(6, 8));
            tags.Pop().Should().BeEquivalentTo(new ItalicsTag(0, 2));
        }

        [Test]
        public void GetAllTags_RecognizesManyTagsOfDifferentTypes()
        {
            var text = "#a\n _c_";
            var tags = TagsConverter.GetAllTags(text);

            tags.Count.Should().Be(2);
            tags.Pop().Should().BeEquivalentTo(new ItalicsTag(4, 6));
            tags.Pop().Should().BeEquivalentTo(new TitleTag(0, 2));
        }

        [Test]
        public void GetAllTags_RecognizesNestedTags()
        {
            var text = "#_a_";
            var tags = TagsConverter.GetAllTags(text);

            tags.Count.Should().Be(2);
            tags.Pop().Should().BeEquivalentTo(new TitleTag(0, 4));
            tags.Pop().Should().BeEquivalentTo(new ItalicsTag(1, 3));
        }
    }
}