using FluentAssertions;
using Markdown.Converters;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class MdTests
    {
        [Test]
        public void Render_ReturnsEmptyString_WhenTextIsNull()
        {
            Md.Render(null).Should().Be("");
        }

        [TestCase("", TestName = "text is empty")]
        [TestCase("    ", TestName = "text is whitespace")]
        [TestCase("ahahahah", TestName = "text not contains tags")]
        [TestCase("_", TestName = "text contains not closed tag")]
        public void Render_ReturnsString_WhenItNotContainsTags(string text)
        {
            Md.Render(text).Should().BeEquivalentTo(text);
        }

        [TestCase("_a_", "<em>a</em>", TestName = "italics single tag")]
        [TestCase("#a\n", "<h1>a</h1>", TestName = "title closed single tag")]
        [TestCase("#a", "<h1>a</h1>", TestName = "title not closed single tag")]
        [TestCase("__a__", "<strong>a</strong>", TestName = "strong text single tag")]
        [TestCase("_a_ _a_ _a_", "<em>a</em> <em>a</em> <em>a</em>", TestName = "many tags of the same type")]
        [TestCase("_a_ #a\n __a__", "<em>a</em> <h1>a</h1> <strong>a</strong>",
            TestName = "many tags of different type")]
        [TestCase("#__ _a_ __", "<h1><strong> <em>a</em> </strong></h1>", TestName = "intersected tags")]
        public void Render_HandlesTagsCorrectly(string text, string expectedResult)
        {
            Md.Render(text).Should().BeEquivalentTo(expectedResult);
        }
    }
}