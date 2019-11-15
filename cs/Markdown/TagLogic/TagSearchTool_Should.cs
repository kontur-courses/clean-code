using System;
using System.Linq;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    class TagSearchTool_Should
    {
        [Test]
        public void GetMarkdownTags_ArgumentNullException_WhenInputStringIsNull()
        {
            Action act = () => TagSearchTool.GetMarkdownTags(null, new string[] { "_" }.ToList());
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void GetMarkdownTags_ReturnEmptyDictionary_WhenInputSeparatorsTagsCountIsZero()
        {
            var foundTags = TagSearchTool.GetMarkdownTags("_a_", new List<string>());

            foundTags.Count.Should().Be(0);
        }

        [TestCase("_a", new [] { "_" })]
        [TestCase("__a", new [] { "__" })]
        [TestCase("_a __a", new [] { "_", "__" })]
        [TestCase("__a _a", new [] { "_", "__" })]
        public void GetMarkdownTags_AbleToFind_OpeningTags(string input, string[] tagSymbols)
        {
            var foundTags = TagSearchTool.GetMarkdownTags(input, tagSymbols.ToList());

            foreach (var tagSymbol in tagSymbols)
            {
                var tag = foundTags[tagSymbol].First();
                tag.Type.Should().Be(TagType.Opening);
            }
        }

        [TestCase("a_", new [] { "_" })]
        [TestCase("a__", new [] { "__" })]
        [TestCase("a_ a__", new [] { "_", "__" })]
        [TestCase("a__ a_", new [] { "_", "__" })]
        public void GetMarkdownTags_AbleToFind_ClosingTags(string input, string[] tagSymbols)
        {
            var foundTags = TagSearchTool.GetMarkdownTags(input, tagSymbols.ToList());

            foreach (var tagSymbol in tagSymbols)
            {
                var tag = foundTags[tagSymbol].First();
                tag.Type.Should().Be(TagType.Closing);
            }
        }

        [TestCase("_aa_ __aa__", new [] { "_", "__" })]
        [TestCase("_aa__ __aa_", new [] { "_", "__" })]
        public void GetMarkdownTags_MustDistinguish_SeparatorsDifferentLengths(string input, string[] tagSymbols)
        {
            var foundTags = TagSearchTool.GetMarkdownTags(input, tagSymbols.ToList());

            foreach (var tagSymbol in tagSymbols)
                foreach (var tag in foundTags[tagSymbol])
                    tag.Symbol.Should().Be(tagSymbol);
        }

        [TestCase("_a", new [] { "_", "__" }, 1)]
        [TestCase("__a", new [] { "_", "__" }, 1)]
        [TestCase("_a__", new [] { "_", "__" }, 2)]
        [TestCase("_a__ __a_", new [] { "_", "__" }, 4)]
        [TestCase("___a___", new [] { "_", "__" }, 0)]
        [TestCase("_a _a _a _a", new [] { "_", "__" }, 4)]
        public void GetMarkdownTags_MustFind_AllTagsInString(string input, string[] tagSymbols, int countTagsInString)
        {
            var foundTags = TagSearchTool.GetMarkdownTags(input, tagSymbols.ToList());

            foundTags.Values.SelectMany(tag => tag).Count().Should().Be(countTagsInString);
        }
    }
}
