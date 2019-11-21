using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Markdown.Core;
using Markdown.Core.Parsers;
using Markdown.Core.Rules;

namespace Markdown.Tests
{
    [TestFixture]
    class ParserTests
    {
        private MainParser mainParser;

        [SetUp]
        public void SetUp()
        {
            mainParser = new MainParser(RuleFactory.CreateAllRules());
        }

        [TestCase("_foo_ __bar__", 4, TestName = "WhenTwoTagsInSeries")]
        [TestCase("_foo __bar__ foo_", 4, TestName = "WhenOneTagNestedInAnother")]
        [TestCase("_a_", 2, TestName = "WhenOneTag")]
        [TestCase("foo bar", 0, TestName = "WhenThereAreNoTags")]
        [TestCase("foo _bar_ foo", 2, TestName = "WhenOneTagInsideTheText")]
        [TestCase("_a ", 0, TestName = "WhenThereIsNoClosingTag")]
        [TestCase("_a _ ", 0, TestName = "WhenThereIsSpaceBeforeClosingTag")]
        [TestCase("1_234_", 0, TestName = "WhenInsideLineWithNumbers")]
        public void ParseLine_ShouldFindAllTokens(string line, int expectedCount)
        {
            mainParser.ParseLine(line).Should().HaveCount(expectedCount);
        }

        [TestCase("_foo_", "SingleUnderscore", "SingleUnderscore", TestName = "WhenSingleUnderscore")]
        [TestCase("__foo__", "DoubleUnderscore", "DoubleUnderscore", TestName = "WhenSingleDoubleUnderscore")]
        [TestCase("_ad __foo__ s_", "SingleUnderscore", "DoubleUnderscore", "DoubleUnderscore", "SingleUnderscore",
            TestName = "WhenDoubleEmbeddedInSingle")]
        [TestCase("__a _b_ c__", "DoubleUnderscore", "SingleUnderscore", "SingleUnderscore", "DoubleUnderscore")]
        [TestCase("__a_", TestName = "WhenUnpairedTags")]
        public void ParseLine_ShouldFindCorrectTags(string line, params string[] expectedTags)
        {
            mainParser.ParseLine(line).Select(tagToken => tagToken.Tag.GetType().Name).Should()
                .BeEquivalentTo(expectedTags);
        }

        [TestCase("_a_", 0, 2, TestName = "WhenSingleUnderscore")]
        [TestCase("__a__", 0, 3, TestName = "WhenDoubleUnderscore")]
        [TestCase("_a __f__ s_", 0, 3, 6, 10, TestName = "WhenDoubleEmbeddedInSingle")]
        public void ParseLine_ShouldFindCorrectPositions(string line, params int[] expectedPositions)
        {
            mainParser.ParseLine(line).Select(tagToken => tagToken.StartPosition).Should()
                .BeEquivalentTo(expectedPositions);
        }
    }
}