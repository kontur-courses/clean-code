using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_Should
    {
        [TestCase("# Header", "<h1> Header</h1>", TestName = "Header lvl 1")]
        [TestCase("## Header", "<h2> Header</h2>", TestName = "Header lvl 2")]
        [TestCase("### Header", "<h3> Header</h3>", TestName = "Header lvl 3")]
        [TestCase("#### Header", "<h4> Header</h4>", TestName = "Header lvl 4")]
        [TestCase("##### Header", "<h5> Header</h5>", TestName = "Header lvl 5")]
        [TestCase("###### Header", "<h6> Header</h6>", TestName = "Header lvl 6")]
        [TestCase("_italic text_", "<em>italic text</em><br><br>", TestName = "Italic tags")]
        [TestCase("__bold text__", "<strong>bold text</strong><br><br>", TestName = "Bold tags")]
        [TestCase("__bold _italic and bold_ text__", "<strong>bold <em>italic and bold</em> text</strong><br><br>",
            TestName = "Italic inside bold")]
        [TestCase("_italic __italic and bold__ text_", "<em>italic <strong>italic and bold</strong> text</em><br><br>",
            TestName = "Bold inside italic")]
        [TestCase("\\_escaped\\_", "_escaped_<br><br>", TestName = "Escaped italic tags")]
        [TestCase("\\_\\_escaped\\_\\_", "__escaped__<br><br>", TestName = "Escaped bold tags")]
        [TestCase("_12_3", "_12_3<br><br>", TestName = "Ignore italic tags inside numbers")]
        [TestCase("__12__3", "__12__3<br><br>", TestName = "Ignore bold tags inside numbers")]
        [TestCase("__different _tags", "__different _tags<br><br>", TestName = "Ignore unpaired tags")]
        [TestCase("sample_ text_", "sample_ text_<br><br>",TestName = "Ignore if first tag is before whitespace")]
        [TestCase("_sample text _", "_sample text _<br><br>", TestName = "Ignore if second tag is after whitespace")]
        public void RenderCorrectly(string paragraph, string expectedHtml)
        {
            var md = new Md();
            // var result = md.Render(paragraph);
            md.Render(paragraph).Should().Be(expectedHtml);
        }
    }
}