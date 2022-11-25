using Markdown;
using FluentAssertions;
using NUnit.Framework;
using System.Diagnostics;
using System.Linq;

namespace Markdown_tests
{
    [TestFixture]
    public class Tests
    {
        [TestCase(@"text _italic text_ text", 
            @"text <em>italic text</em> text", 
            TestName = "{m}_Italic")]
        [TestCase(@"text __bold _italic_ bold__ text.", 
            @"text <strong>bold <em>italic</em> bold</strong> text.", 
            TestName = "{m}_InnerItalic")]
        [TestCase(@"text __bold _italic_bold bold__ text.", 
            @"text <strong>bold <em>italic</em>bold bold</strong> text.", 
            TestName = "{m}_InnerItalicInOneWordOnBoard")]
        [TestCase(@"text __bold bold_italic_bold bold__ text.", 
            @"text <strong>bold bold<em>italic</em>bold bold</strong> text.", 
            TestName = "{m}_InnerItalicInOneWord")]
        [TestCase(@"text _italic __italic__ italic_ text.", 
            @"text <em>italic __italic__ italic</em> text.", 
            TestName = "{m}_InnerBold")]
        [TestCase(@"__Bold__ text.", 
            @"<strong>Bold</strong> text.", 
            TestName = "{m}_Bold")]
        [TestCase(@"text \_text\_ text.",
            @"text _text_ text.", 
            TestName = "{m}_ShieldingSlash")]
        [TestCase(@"text t\ext\ \text\",
            @"text t\ext\ \text\", 
            TestName = "{m}_NonShieldingSlash")]
        [TestCase(@"text \\_italic_ text",
            @"text \<em>italic</em> text", 
            TestName = "{m}_SlashShieldingSlash")]
        [TestCase(@"\\\_text_",
            @"\_text_", 
            TestName = "{m}_TripleSlash")]
        [TestCase(@"text_12_3", 
            @"text_12_3", 
            TestName = "{m}_ConcatinationWithDigits")]
        [TestCase(@"text _italic_text text_italic_text text_italic_", 
            @"text <em>italic</em>text text<em>italic</em>text text<em>italic</em>", 
            TestName = "{m}_ItalicInSameWord")]
        [TestCase(@"te_xt te_xt", 
            @"te_xt te_xt", 
            TestName = "{m}_ItalicInDifferentWord")]
        [TestCase(@"text _italic it_alic it_alic italic_",
            @"text <em>italic it_alic it_alic italic</em>", 
            TestName = "{m}_ItalicDifferentWordsAndRightWrap")]
        [TestCase(@"text_ text_ text", 
            @"text_ text_ text", 
            TestName = "{m}_WhiteSpaceAfterOpeningModifier")]
        [TestCase(@"text _text _text", 
            @"text _text _text", 
            TestName = "{m}_ModifierAfterSpace")]
        [TestCase(@"text __text _text__ text_ text", 
            @"text __text _text__ text_ text",
            TestName = "{m}_OverlapModifiers")]
        [TestCase(@"# title",
            @"<h1> title</h1>", 
            TestName = "{m}_Title")]
        [TestCase(@"# title __bold _italic_ bold__",
            @"<h1> title <strong>bold <em>italic</em> bold</strong></h1>", 
            TestName = "{m}_TitleWithModifiers")]
        [TestCase(@"text [link name](www.link.com) text",
            @"text <a href=""www.link.com"">link name</a> text",
            TestName = "{m}_LinkInText")]
        [TestCase(@"text [link name](www.l_in_k.com) text",
            @"text <a href=""www.l_in_k.com"">link name</a> text",
            TestName = "{m}_LinkWithItalic")]
        [TestCase(@"text \[link name](www.link.com) text",
            @"text [link name](www.link.com) text",
            TestName = "{m}_LinkWithStartSlash")]
        [TestCase(@"text [link name] (www.link.com) text",
            @"text [link name] (www.link.com) text",
            TestName = "{m}_NonValidLink")]

        public void Md_CommonInput_ShouldBeExpected(string md, string exp)
        {
            var markdown = new Md();

            var html = markdown.Render(md);

            html.Should().Be(exp);
        }

        [TestCase(@"", @"", TestName = "EmptyInput")]
        [TestCase(@"  ", @"  ", TestName = "OnlyWhitespacesInput")]
        [TestCase(@"_", @"_", TestName = "OnlyItalic")]
        [TestCase(@"__", @"__", TestName = "OnlyBold")]
        public void Md_BorderlineCases_ShouldBeExpected(string md, string exp)
        {
            var markdown = new Md();

            var html = markdown.Render(md);

            html.Should().Be(exp);
        }

        [Test]
        public void Md_Performance_ShouldBeLinear()
        {
            var markdown = new Md();
            const string line = @"_простой_ __текст__ \_имеющий\_ разные символы";
            var lastTime = 0L;

            for (int i = 1; i < 10; i++)
            {
                var testLine = string.Concat(Enumerable.Repeat(line, i));

                var sw = new Stopwatch();
                sw.Start();

                var html = markdown.Render(testLine);

                sw.Stop();

                if (i == 1) lastTime = sw.ElapsedTicks;
                else
                {
                    var currentTime = sw.ElapsedTicks;
                    var dif = currentTime - lastTime;

                    dif.Should().BeLessThan(currentTime);
                }
            }
        }
    }
}