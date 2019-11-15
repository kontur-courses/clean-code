using System;
using FluentAssertions;
using NUnit.Framework;

namespace MarkDown.Tests
{
    [TestFixture]
    public class MdParserTests
    {
        [TestCase("<em>html</em>", "&lt;em&gt;html&lt;/em&gt;")]
        public void ParseFrom_ShouldRemoveHtmlTags(string line, string expectedLine)
        {
            MdParser.ParseFrom(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase(@"\\_a_", @"\<em>a</em>")]
        public void ParseFrom_RemovesExtraEscapeChars(string line, string expectedLine)
        {
            MdParser.ParseFrom(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase("_a__a__a_", "<em>a__a__a</em>")]
        [TestCase("_Hello__World__A__a__A_", "<em>Hello__World__A__a__A</em>")]
        public void ParseFrom_RemovesNestedTags(string line, string expectedLine)
        {
            MdParser.ParseFrom(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase("__a_a_a__", "<strong>a<em>a</em>a</strong>")]
        public void ParseFrom_NotRemovesAllowedNestedTags(string line, string expectedLine)
        {
            MdParser.ParseFrom(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase("_hello__world_a__", "<em>hello__world</em>a__")]
        [TestCase("__hello_world__a_", "<strong>hello<em>world</em></strong><em>a</em>")]
        public void ParseFrom_HandlesCrossedTagsCorrectly(string line, string expectedLine)
        {
            MdParser.ParseFrom(line).Should().BeEquivalentTo(expectedLine);
        }

        //What should I use to count exact time threshold for performance test?
    }
}