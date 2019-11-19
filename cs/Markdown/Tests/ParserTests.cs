using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Markdown.Core;
using Markdown.Core.Rules;
using NUnit.Framework.Internal;

namespace Markdown.Tests
{
    [TestFixture]
    class ParserTests
    {
        private IEnumerable<IRule> rules = RuleFactory.CreateAllRules();

        [TestCase("_foo_ __bar__", 4, TestName = "WhenTwoTagsInSeries")]
        [TestCase("_foo __bar__ foo_", 4, TestName = "WhenOneTagNestedInAnother")]
        [TestCase("_a_", 2, TestName = "WhenOneTag")]
        [TestCase("foo bar", 0, TestName = "WhenThereAreNoTags")]
        [TestCase("foo _bar_ foo", 2, TestName = "WhenOneTagInsideTheText")]
        [TestCase("_a ", 0, TestName = "WhenThereIsNoClosingTag")]
        [TestCase("_a _ ", 0, TestName = "WhenThereIsSpaceBeforeClosingTag")]
        [TestCase("1_234_", 0, TestName = "WhenInsideLineWithNumbers")]
        [TestCase(@"\_foo\_", 0, TestName = "WhenShielded")]
        [TestCase(@"_foo\_", 0, TestName = "foo")]
        public void ParseLine_ShouldFindAllTokens(string line, int expectedCount)
        {
            Parser.Parse(line, rules).Should().HaveCount(expectedCount);
        }
    }
}
