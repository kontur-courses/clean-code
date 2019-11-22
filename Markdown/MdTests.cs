using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    class MdTests
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [Test]
        public void Render_OnEmptyString()
        {
            md.Render("").Should().Be("");
        }

        [Test]
        public void Render_OnNullString()
        {
            md.Render(null).Should().Be("");
        }

        [Test]
        public void Render_OnSimpleString()
        {
            md.Render("abcd").Should().Be("<div>abcd</div>");
        }

        [TestCase("__abcd__", "<div><strong>abcd</strong></div>", TestName = "One tag on whole string")]
        [TestCase("__ab_c__d_", "<div><strong>ab_c</strong>d_</div>", TestName = "Italic and bold intersect")]
        [TestCase("___ab___", "<div><strong><em>ab</em></strong></div>", TestName = "Italic in Bold")]
        [TestCase("a5__ba__", "<div>a5_<em>ba</em>_</div>", TestName = "Number near tag")]
        [TestCase("___", "<div>___</div>", TestName = "Underline")]
        [TestCase("\\__ab__", "<div>_<em>ab</em>_</div>", TestName = "First underline is escaped")]
        [TestCase("\\_ab_", "<div>_ab_</div>", TestName = "Open Italic Tag is escaped")]
        public void Render_OnBoldAndItalicTag(string input, string output)
        {
            md.Render(input).Should().Be(output);
        }

        [TestCase("# ABCD", "<div><h1>ABCD</h1></div>", TestName = "H1 tag")]
        [TestCase("## ABCD", "<div><h2>ABCD</h2></div>", TestName = "H2 tag")]
        [TestCase("# ABCD\nAB", "<div><h1>ABCD</h1>\nAB</div>", TestName = "TextAfterHeader")]
        [TestCase("# AB __b__\n", "<div><h1>AB __b__</h1>\n</div>", TestName = "Ignore tags in header")]
        public void Render_OnHeaders(string input, string output)
        {
            md.Render(input).Should().Be(output);
        }

        [TestCase("```\nabc\n```", "<div><pre>abc</pre></div>")]
        [TestCase("\\```ab```", "<div>```ab```</div>", TestName = "Escaped begin of the openTag")]
        [TestCase("```\n```\nab\n```\n```", "<div><pre>```\nab</pre>\n```</div>")]
        public void Render_OnRawTag(string input, string output)
        {
            md.Render(input).Should().Be(output);
        }
    }
}
