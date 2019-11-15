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

        public void ParseFrom_RemovesNestedTags(string line, string expectedLine) //TODO
        {
            throw new NotImplementedException();
        }
    }
}