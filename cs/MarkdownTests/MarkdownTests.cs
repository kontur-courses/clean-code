using System;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MarkdownTests
    {
        [TestCase("", "", TestName = "empty string")]
        [TestCase("text", "text", TestName = "no tags")]
        [TestCase("_text_", "<em>text</em>", TestName = "em")]
        [TestCase(@"\_text\_", "_text_", TestName = "escaped symbols")]
        [TestCase("__text__", "<strong>text</strong>", TestName = "strong")]
        [TestCase("__text1 _text2_ text3__", "<strong>text1 <em>text2</em> text3</strong>", TestName = "em inside strong")]
        [TestCase("_text1 __text2__ text3_", "<em>text1 <strong>text2</strong> text3</em>", TestName = "strong inside em")]
        [TestCase("text_1_23", "text_1_23", TestName = "tags inside text with numbers")]
        [TestCase("__text1 _text2", "__text1 _text2", TestName = "opening tags with no closing")]
        [TestCase("text1_ text2_", "text1_ text2_", TestName = "tags followed by space")]
        [TestCase("_text1 _text2 text3_", "<em>text1 _text2 text3</em>", TestName = "tags preceded by space")]
        [TestCase("__text1 _text2__ text3_", "<strong>text1 _text2</strong> text3_", TestName = "intersecting tags")]
        [TestCase("___text__", "<strong>_text</strong>", TestName = "clustered underscores")]
        [TestCase("qw__ er __ty", "qw__ er __ty", TestName = "strong tag preceded and followed by space")]
        [TestCase("__q _b __c__ d_ w__", "<strong>q _b __c</strong> d_ w__", TestName = "strong inside em inside strong")]
        [TestCase(@"\\_a_", @"\<em>a</em>", TestName = "escaped backslash")]
        public void Render_ShouldWorkCorrectlyOn(string input, string expectedResult)
        {
            var result = Markdown.Render(input);

            result.Should().Be(expectedResult);
        }

        [Test]
        public void Render_ShouldThrowArgumentNullException()
        {
            Action render = () => Markdown.Render(null);

            render.ShouldThrow<ArgumentNullException>();
        }
    }
}
