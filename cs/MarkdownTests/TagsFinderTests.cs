using FluentAssertions;
using Markdown.MarkerLogic;
using Markdown.TagClasses;
using Markdown.TagClasses.ITagInterfaces;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    internal class TagsFinderTests
    {
        private TagsFinder sut = new();

        [TestCase("aa_a1")]
        [TestCase("aa__a1")]
        public void CreateTagList_GiveWordWithDigits_ShouldIgnorePairedTagsInMiddleOfIt(string text)
        {
            var tagList = sut.CreateTagList(text);

            tagList.Count.Should().Be(0);
        }

        [TestCase(@"\\#aaa")]
        [TestCase("a#aa")]
        [TestCase("aaa#")]
        public void CreateTagList_GiveWordWithNotEscapedSharpNotInBeginning_ShouldIgnoreIt(string text)
        {
            var tagList = sut.CreateTagList(text);

            tagList.Count.Should().Be(0);
        }

        [Test]
        public void CreateTagList_GiveWordWithPairedTagAfterHeader_ShouldMarkItAsStarterTag()
        {
            var expectedResult = new List<ITag>
            {
                new HeaderTag(0, TagType.Header),
                new PairedTag(1, TagType.Emphasis, true)
            };

            var tagList = sut.CreateTagList("#_aaa");

            tagList.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateTagList_GiveWordWithInconclusiveTagsAtTheBeginnigOfWord_ShouldPlaceStrongTagFirst()
        {
            var expectedResult = new List<ITag>
            {
                new PairedTag(0, TagType.Strong, true),
                new PairedTag(2, TagType.Emphasis, true)
            };

            var tagList = sut.CreateTagList("___aaa");

            tagList.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateTagList_GiveWordWithInconclusiveTagsAtTheEndOfWord_ShouldPlaceStrongTagLast()
        {
            var expectedResult = new List<ITag>
            {
                new PairedTag(3, TagType.Emphasis, false, true),
                new PairedTag(4, TagType.Strong, false, true)
            };

            var tagList = sut.CreateTagList("aaa___");

            tagList.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateTagList_GiveWordWithTagsInMiddle_ShouldMarkItAsCanBeStarterAndEnder()
        {
            var expectedResult = new List<ITag>
            {
                new PairedTag(1, TagType.Emphasis, true, true)
            };

            var tagList = sut.CreateTagList("a_aa");

            tagList.Should().BeEquivalentTo(expectedResult);
        }

        [TestCase(@"a\_aa", 2, Description = "In Middle")]
        [TestCase(@"\_aaa", 1, Description = "At beginning")]
        [TestCase(@"aaa\_", 4, Description = "At end")]
        public void CreateTagList_GiveWordWithEscapedPairedTag_ShouldMarkItAsEscapedAndCantBeStarterOrEnder(string text,
            int position)
        {
            var expectedResult = new List<ITag>
            {
                new PairedTag(position, TagType.Emphasis, isEscaped: true)
            };

            var tagList = sut.CreateTagList(text);

            tagList.Should().BeEquivalentTo(expectedResult);
        }

        [TestCase(@"_aa", 0, Description = "Nothing before tag")]
        [TestCase(@"aa _aa", 4, Description = "Space before tag")]
        public void CreateTagList_ShouldNotMarkAsEnder(string text, int position)
        {
            var expectedResult = new List<ITag>
            {
                new PairedTag(position, TagType.Emphasis, canBeStarter: true, canBeEnder: false)
            };

            var tagList = sut.CreateTagList(text);

            tagList.Should().BeEquivalentTo(expectedResult);
        }

        [TestCase(@"aa_", 2, Description = "Nothing after tag")]
        [TestCase(@"aa_ aa", 2, Description = "Space after tag")]
        public void CreateTagList_ShouldNotMarkAsStarter(string text, int position)
        {
            var expectedResult = new List<ITag>
            {
                new PairedTag(position, TagType.Emphasis, canBeStarter: false, canBeEnder: true)
            };

            var tagList = sut.CreateTagList(text);

            tagList.Should().BeEquivalentTo(expectedResult);
        }
    }
}