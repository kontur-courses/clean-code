using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace TextFormattersTests
{
    [TestFixture]
    public class MarkdownTests
    {
        [SetUp]
        public void SetUp()
        {
            markdown = new Md();
        }

        [TearDown]
        public void TearDown()
        {
        }

        private Md markdown;

        [TestCase("", "")]
        [TestCase("_", "_")]
        [TestCase("_hello_", "<em>hello</em>")]
        [TestCase("_hello", "_hello")]
        [TestCase("__hello__", "<strong>hello</strong>")]
        [TestCase("__hello", "__hello")]
        [TestCase("ab_cd_ef", "ab<em>cd</em>ef")]
        [TestCase("_hello __world__ bye_", "<em>hello __world__ bye</em>")]
        [TestCase("# Title", "<h1>Title</h1>")]
        [TestCase("## Title", "<h2>Title</h2>")]
        [TestCase("# Title\n_body_", "<h1>Title</h1>\n<em>body</em>")]
        [TestCase("# Title __with _different_ tags__", "<h1>Title <strong>with <em>different</em> tags</strong></h1>")]
        [TestCase("#NotTitle", "#NotTitle")]
        [TestCase("__hello _world_ !__", "<strong>hello <em>world</em> !</strong>")]
        [TestCase(@"\_hello\_", "_hello_")]
        [TestCase(@"hel\lo", @"hel\lo")]
        [TestCase(@"\\_hello_", @"\<em>hello</em>")]
        [TestCase("* hello\n* world", "<ul><li>hello</li>\n<li>world</li></ul>")]
        public void SpecificationTests(string md, string html)
        {
            var actual = markdown.Render(md);
            actual.Should().Be(html);
        }
    }
}
