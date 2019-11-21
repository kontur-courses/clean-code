using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace Markdown
{
    [TestFixture]
    internal class TagParserTests
    {
        private static TagParser tagParser;

        [SetUp]
        public void SetUp() => tagParser = new TagParser(Markdown.TagsInfo.Keys.ToArray());
        
        [TestCase("_a")]
        [TestCase("__a")]
        [TestCase("_a __a")]
        [TestCase("__a _a")]
        public void FindOpeningTags(string input)
        {
            var foundTags = tagParser.Parse(input);

            foreach (var tag in foundTags)
                tag.Type.Should().Be(TagType.Opening);
        }

        [TestCase("a_")]
        [TestCase("a__")]
        [TestCase("a_ a__")]
        [TestCase("a__ a_")]
        public void FindClosingTags(string input)
        {
            var foundTags = tagParser.Parse(input);

            foreach (var tag in foundTags)
                tag.Type.Should().Be(TagType.Closing);
        }

        [TestCase("_a", 1, TestName = "OneEmphasisTag")]
        [TestCase("__a", 1, TestName = "OneStrongTag")]
        [TestCase("_a__", 2, TestName = "OneStrongOneEmphasisTags")]
        [TestCase("_a__ __a_", 4, TestName = "TwoStrongTwoEmphasisTags")]
        [TestCase("_a _a _a _a", 4, TestName = "FourEmphasisTags")]
        public void FindAllTagsInString(string inputString, int countTagsInString)
        {
            var foundTags = tagParser.Parse(inputString);

            foundTags.Should().HaveCount(countTagsInString);
        }
        
        [TestCase("___a___", TestName = "ZeroCorrectTags")]
        [TestCase("aa_aa", TestName = "EmphasisTagBetweenLetters")]
        [TestCase("aa__aa", TestName = "StrongTagBetweenLetters")]
        [TestCase(" __ aa", TestName = "WhitespaceToLeftAndRightTag")]
        public void StringWithoutTags(string inputString)
        {
            var foundTags = tagParser.Parse(inputString);

            foundTags.Should().HaveCount(0);
        }
    }
}
