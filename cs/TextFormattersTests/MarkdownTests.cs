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
        [TestCase("____", "____")]
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
        [TestCase("hel_lo worl_d", "hel_lo worl_d")]
        [TestCase("hel_1_o", "hel_1_o")]
        [TestCase(@"hel\lo", @"hel\lo")]
        [TestCase(@"\\_hello_", @"\<em>hello</em>")]
        [TestCase("__hello_ __world_", "__hello_ __world_")]
        [TestCase("_hello__ _world__", "_hello__ _world__")]
        [TestCase("*hello\n*world* 1\n*2", "<ul><li>hello</li>\n<li>world* 1</li>\n<li>2</li></ul>")]
        [TestCase("*he_ll_o\n*world", "<ul><li>he<em>ll</em>o</li>\n<li>world</li></ul>")]
        [TestCase("*hello\n*world\n", "<ul><li>hello</li>\n<li>world</li>\n</ul>")]
        [TestCase("**Why?", "<ul><li>*Why?</li></ul>")]
        [TestCase("*First\n---\n*Second", "<ul><li>First</li>\n</ul>---\n<ul><li>Second</li></ul>")]
        public void SpecificationTests(string md, string html)
        {
            var actual = markdown.Render(md);
            Assert.AreEqual(html, actual);
        }
    }
}
