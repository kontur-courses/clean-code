using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MarkDown.TagParsers;
using MarkDown.Tokens;
using NUnit.Framework;

namespace MarkDown.Tests
{
    public class TagTokenizerTests
    {
        [TestCase("Test line")]
        [TestCase("Hello, World!")]
        public void DivideTokensByTag_ReturnsTheSameWithoutMdTags(string line)
        {
            var preparedLine = PrepareLine(line);
            TagTokenizer.DivideTokensByTag(preparedLine, new EmTag()).Should().BeEquivalentTo(preparedLine);
        }

        [Test]
        public void DivideTokensByTag_DividesTokensCorrectly()
        {
            var preparedLine = PrepareLine("_Hello, World!_");
            var expectedAnswer = new List<MdToken>
            {
                new TagToken(new EmTag()),
                new StringToken("Hello, World!", "Hello, World!".Length),
                new TagToken(new EmTag())
            };
            TagTokenizer.DivideTokensByTag(preparedLine, new EmTag()).ToList().Should().BeEquivalentTo(expectedAnswer);
        }

        [Test]
        public void DivideTokensByTag_HandlesNestedTags()
        {
            var preparedLine = PrepareLine("_Hello, __World!___");
            var expectedAnswer = new List<MdToken>
            {
                new TagToken(new EmTag()),
                new StringToken("Hello, ", "Hello, ".Length),
                new TagToken(new StrongTag()),
                new StringToken("World!", "World!".Length),
                new TagToken(new StrongTag()),
                new TagToken(new EmTag())
            };
            var firstDivision = TagTokenizer.DivideTokensByTag(preparedLine, new StrongTag());
            var secondDivision = TagTokenizer.DivideTokensByTag(firstDivision, new EmTag()).ToList();
            secondDivision.Should().BeEquivalentTo(expectedAnswer);
        }

        private IEnumerable<MdToken> PrepareLine(string line)
        {
            return TagParser.Tokenize(line);
        }
    }
}