using System;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MdTests
    {
        private Md md;
        [SetUp]
        public void Setup()
        {
            md = new Md();
        }
        [TestCase("[text [otherText](otherLink)](link)", "[text [otherText](otherLink)](link)", TestName = "Link text contains other link tags")]
        [TestCase("[text](link)", "<a href=\"link\">text</a>", TestName = "Text without other tags")]
        [TestCase("[_text_](link)", "<a href=\"link\">\\<em>text\\</em></a>", TestName = "Text with italic tag")]
        [TestCase("[__text__](link)", "<a href=\"link\">\\<strong>text\\</strong></a>", TestName = "Text with strong tag")]
        [TestCase("[__text _text_ text__](link)", "<a href=\"link\">\\<strong>text \\<em>text\\</em> text\\</strong></a>", TestName = "Text with italic in strong tags")]
        public void Render_ReturnCorrectHtml_WhenTextContainsOnlyLinkTag(string text, string expectedHtml)
        {
            var html = md.Render(text);

            html.Should().BeEquivalentTo(expectedHtml);
        }
        
        [TestCase("__strong _italic_ strong__", "\\<strong>strong \\<em>italic\\</em> strong\\</strong>", TestName = "italic in strong formatting tag works")]
        [TestCase("_italic __strong__ italic_", "_italic __strong__ italic_", TestName = "strong in italic tag formatting does not work")]//------
        [TestCase("italic_1_2_3", "italic_1_2_3", TestName = "underscore with numbers is not formatted")]
        [TestCase("italic_1_2_3", "italic_1_2_3", TestName = "underscore with numbers is not formatted")]
        
        [TestCase("_it_alic", "\\<em>it\\</em>alic", TestName = "italic tag at the begin word works")]
        [TestCase("it_al_ic", "it\\<em>al\\</em>ic", TestName = "italic tag in the middle word works")]
        [TestCase("ital_ic_", "ital\\<em>ic\\</em>", TestName = "italic tag in the end word works")]
        
        [TestCase("diff_erent wor_ds", "diff_erent wor_ds", TestName = "italic tag in different words word does not works")]
        
        [TestCase("__not paired strong italic_", "__not paired strong italic_", TestName = "not paired(strong-italic) tags does not works")]
        [TestCase("_not paired italic strong__", "_not paired italic strong__", TestName = "not paired(italic-strong) tags does not works")]
        
        [TestCase("it_ wrong_", "it_ wrong_", TestName = "formatting without whitespace before beginning italic tag does not work")]
        [TestCase("_underline _notWorks", "_underline _notWorks", TestName = "formatting with whitespace before end italic tag does not work")]
        [TestCase("it__ wrong__", "it__ wrong__", TestName = "formatting without whitespace before beginning strong tag does not work")]
        [TestCase("__underline __notWorks", "__underline __notWorks", TestName = "formatting with whitespace before end strong tag does not work")]
        
        [TestCase("__strong _italic__ word_", "__strong _italic__ word_", TestName = "intersect strong-italic does not works")]
        [TestCase("_italic __strong_ word__", "_italic __strong_ word__", TestName = "intersect italic-strong does not works")]
        
        [TestCase("____", "____", TestName = "empty strong tag is not formatted")]
        [TestCase("__", "__", TestName = "empty italic tag is not formatted")]
        [TestCase("", "", TestName = "empty text return empty")]
        public void Render_ReturnCorrectHtml_TagsInteraction(string text, string expectedHtml)
        {
            var html = md.Render(text);

            html.Should().BeEquivalentTo(expectedHtml);
        }
        
        [TestCase("\\[text](link)", "text](link)", TestName = "Text with escaped link tag")]
        [TestCase("\\_italic_", "italic_", TestName = "Text with escaped italic tag")]
        [TestCase("\\__strong__", "strong__", TestName = "Text with escaped strong tag")]
        [TestCase("\\#head", "head", TestName = "Text with escaped head tag")]
        public void Render_ReturnCorrectHtml_WhenTextContainsEscapingSymbol(string text, string expectedHtml)
        {
            var html = md.Render(text);

            html.Should().BeEquivalentTo(expectedHtml);
        }

        [Test]
        public void Render_TenThousandAboutTenTimesLessThanOneHundredThousand_AlgorithmComplexity()
        {
            var stopwatch = new Stopwatch();
            var builder = new StringBuilder();
            for (int i = 0; i < 10000; i++)
            {
                builder.Append("#_aaaa_ __bbb__");
                builder.Append(Environment.NewLine);
            }
            
            stopwatch.Start();
            var html = md.Render(builder.ToString());
            stopwatch.Stop();
            Console.WriteLine("100 : " + stopwatch.Elapsed);

            var tenThousand = stopwatch.Elapsed;
            for (int i = 0; i < 100000; i++)
            {
                builder.Append("#_aaaa_ __bbb__");
                builder.Append(Environment.NewLine);
            }
            
            stopwatch.Start();
            html = md.Render(builder.ToString());
            stopwatch.Stop();
            Console.WriteLine("1000 : " + stopwatch.Elapsed);
            var oneHundredThousand = stopwatch.Elapsed;
            var result = oneHundredThousand/tenThousand;
            Console.WriteLine("result = " + result);

            result.Should().BeLessThan(20);

        }
        
    }
}