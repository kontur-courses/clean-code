using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
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
        [TestCase("\\__ab__", "<div>_<em>ab</em>_</div>")]
        public void Render_OnBoldTag(string input, string output)
        {
            var actual = md.Render(input);
            actual.Should().Be(output);
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
        public void Render_OnRawTag(string input, string output)
        {
            md.Render(input).Should().Be(output);
        }
    }
}
