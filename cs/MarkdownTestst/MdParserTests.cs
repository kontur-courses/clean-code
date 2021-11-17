using FluentAssertions;
using Markdown;
using Markdown.Renderer;
using NUnit.Framework;

namespace MarkdownTests
{
    public class Tests
    {
        public Md Md;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Md = new Md(new HtmlRenderer());
        }

        [TestCase("abc", "abc")]
        [TestCase("# abc", "<h1> abc\\<h1>")]
        [TestCase("\\_a_", "_a_")]
        [TestCase("_a__a__a_", "<em>a__a__a\\<em>")]
        [TestCase("__a _a_ a__", "<strong>a <em>a\\<em> a\\<strong>")]
        [TestCase("# __a _a_ a__", "<h1> <strong>a <em>a\\<em> a\\<strong>\\<h1>")]
        public void Test1(string input, string expected)
        {
            var actual = Md.Render(input);

            actual.Should().Be(expected);
        }
    }
}