using FluentAssertions;
using MarkDown.TagParsers;
using NUnit.Framework;

namespace MarkDown.Tests
{
    public class StrongParserTests
    {
        [TestCase("", TestName = "OnEmptyString")]
        [TestCase("asdfasdf", TestName = "OnLineWithoutTags")]
        [TestCase("__a", TestName = "OnLineWithoutPairTags")]
        [TestCase("__a__", TestName = "OnLineWithTags")]

        public void GetParsedLine__ReturnsNotNull(string line)
        {
            GetParsedLine(line).Should().NotBeNull();
        }

        [TestCase("__a__", "<strong>a</strong>")]
        [TestCase("__aabb__", "<strong>aabb</strong>")]
        [TestCase("Hello __world__", "Hello <strong>world</strong>")]
        [TestCase("__Hello__ __world__", "<strong>Hello</strong> <strong>world</strong>")]
        public void GetParsedLine__ParseSimpleFieldsCorrectly(string line, string expectedLine)
        {
            GetParsedLine(line).Should().BeEquivalentTo(expectedLine);
        }


        [TestCase("____", "____")]
        [TestCase("Hello __world__ ____", "Hello <strong>world</strong> ____")]
        [TestCase("____ Hello __world__", "<strong>__ Hello __world</strong>")]
        public void GetParsedLine__NotParsesZeroLengthTags(string line, string expectedLine)
        {
            GetParsedLine(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase("\\__Hello world__", "__Hello world__")]
        [TestCase("Hello \\__world__", "Hello __world__")]
        [TestCase("__hello\\__ __world__", "<strong>hello__ __world</strong>")]
        public void GetParsedLine__NotParsesEscapedTags(string line, string expectedLine)
        {
            GetParsedLine(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase("__a __a__ __a__", "<strong>a __a</strong> <strong>a</strong>")]
        public void GetParsedLine__ChoosesShortestPossibleTag(string line, string expectedLine)
        {
            GetParsedLine(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase(@"\\__a__", @"\<strong>a</strong>")]
        [TestCase(@"__a\\__", @"<strong>a\</strong>")]
        public void GetParsedLine__ParsesDoubleEscapeChar(string line, string expectedLine)
        {
            GetParsedLine(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase("1__2", "1__2")]
        [TestCase("1__f", "1__f")]
        [TestCase("f__2", "f__2")]
        public void GetParsedLine__NotParsesDigits(string line, string expectedLine)
        {
            GetParsedLine(line).Should().BeEquivalentTo(expectedLine);
        }

        private string GetParsedLine(string inputLine)
        {
            return Md.Render(inputLine);
        }
    }
}