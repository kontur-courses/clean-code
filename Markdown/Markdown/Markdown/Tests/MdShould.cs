using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown.Markdown;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MdShould
    {

        [TestCase("some _text_", "some \\<em>text\\</em>", TestName = "Simple Italics Test")]
        [TestCase("__text__", "\\<strong>text\\</strong>", TestName = "Simple Strong Test")]
        [TestCase("_Field\\_", "_Field_", TestName = "Simple header test")]
        [TestCase("# some text\n", "\\<h1>some text\\</h1>", TestName = "Simple Header Test")]
        [TestCase("some \\_text\\_", "some _text_", TestName = "Shielded Italics Test")]
        [TestCase("__text\\__", "__text__", TestName = "Shielded Strong Test")]
        [TestCase("\\# text \\\n", "# text \n", TestName = "Shielded Header Test")]
        [TestCase("some \\\\_text_", "some \\\\<em>text\\</em>", TestName = "Shielded shield Test")]
        [TestCase("some \\\\text", "some \\text", TestName = "Empty shield Test")]
        [TestCase("__e some _text_ e__", "\\<strong>e some \\<em>text\\</em> e\\</strong>", TestName = "Italics inside strong test")]
        [TestCase("_e some __text__ e_", "\\<em>e some __text__ e\\</em>", TestName = "Strong inside italics test")]
        [TestCase("some _1_2__3__text", "some _1_2__3__text", TestName = "Underlined numbers test")]
        [TestCase("_so_me c_rin_ge te_xt_ __fo__r b__r__o ro__fl__", "\\<em>so\\</em>me c\\<em>rin\\</em>ge te\\<em>xt\\</em> \\<strong>fo\\</strong>r b\\<strong>r\\</strong>o ro\\<strong>fl\\</strong>", TestName = "One word test")]
        [TestCase("__some text for rof__l", "__some text for rof__l", TestName = "Different words test")]
        [TestCase("_some text__", "_some text__", TestName = "UnpairedTagsTest")]
        [TestCase("some_ cringe_ text__ bro__", "some_ cringe_ text__ bro__", TestName = "SpaceAfterBeginningTagTest")]
        [TestCase("_some _cringe __text __bro", "_some _cringe __text __bro", TestName = "SpaceBeforeEndingTagTest")]
        [TestCase("_some __cringe_ text__", "_some __cringe_ text__", TestName = "CrossedTagsTest")]


        public void MarkdownRender_WhenStringWithTags(string mdString, string htmlString)
        {
            var result = Md.Render(mdString);
            result.Should().Be(htmlString);
        }

        [Test]
        public void CheckTime()
        {
            var time = new Stopwatch();
            time.Start();
            for (int i = 0; i < 30000; i++)
            {
                Test();
            }
            time.Elapsed.Milliseconds.Should().BeLessThan(1000);
        }
        [Test]
        public void Test()
        {
            var mdString = "# markdown _test_ sentence__\n";
            Md.Render(mdString).Should().Be("\\<h1>markdown \\<em>test\\</em> sentence__\\</h1>");
        }
    }
}
