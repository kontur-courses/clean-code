using System;
using FluentAssertions;
using MarkDown.TagParsers;
using NUnit.Framework;

namespace MarkDown.Tests
{
    [TestFixture]
    public class EmParserTests
    {
        private EmTagParser parser = new EmTagParser();

        [TestCase("", TestName = "OnEmptyString")]
        [TestCase("asdfasdf", TestName = "OnLineWithoutTags")]
        [TestCase("_a", TestName = "OnLineWithoutPairTags")]
        [TestCase("_a_", TestName = "OnLineWithTags")]

        public void GetParsedLine_ReturnsNotNull(string line)
        {
            parser.GetParsedLineFrom(line).Should().NotBeNull();
        }

        [TestCase("_a_", "<em>a</em>")]
        [TestCase("_aabb_", "<em>aabb</em>")]
        [TestCase("Hello _world_", "Hello <em>world</em>")]
        [TestCase("_Hello_ _world_", "<em>Hello</em> <em>world</em>")]
        public void GetParsedLine_ParseSimpleFieldsCorrectly(string line, string expectedLine)
        {
            parser.GetParsedLineFrom(line).Should().BeEquivalentTo(expectedLine);
        }


        [TestCase("__", "__")]
        [TestCase("Hello _world_ __", "Hello <em>world</em> __")]
        [TestCase("__ Hello _world_", "__ Hello <em>world</em>")]
        public void GetParsedLine_NotParsesZeroLengthTags(string line, string expectedLine)
        {
            parser.GetParsedLineFrom(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase("\\_Hello world_", "\\_Hello world_")]
        [TestCase("Hello \\_world_", "Hello \\_world_")]
        [TestCase("_hello\\_ _world_", "<em>hello\\_ _world</em>")]
        public void GetParsedLine_NotParsesEscapedTags(string line, string expectedLine)
        {
            parser.GetParsedLineFrom(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase("_a _a_ _a_", "<em>a _a</em> <em>a</em>")]
        public void GetParsedLine_ChoosesShortestPossibleTag(string line, string expectedLine)
        {
            parser.GetParsedLineFrom(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase(@"\\_a_", @"\\<em>a</em>")]
        [TestCase(@"_a\\_", @"<em>a\\</em>")]
        public void GetParsedLine_ParsesDoubleEscapeChar(string line, string expectedLine)
        {
            parser.GetParsedLineFrom(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase("1_2", "1_2")]
        [TestCase("1_f", "1_f")]
        [TestCase("f_2", "f_2")]
        public void GetParsedLine_NotParsesDigits(string line, string expectedLine)
        {
            parser.GetParsedLineFrom(line).Should().BeEquivalentTo(expectedLine);
        }
    }
}