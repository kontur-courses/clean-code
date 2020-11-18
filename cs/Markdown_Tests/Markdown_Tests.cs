using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace Markdown_Tests
{
    internal class Markdown_Tests
    {
        private Md markdownToHtml;
        
        [SetUp]
        public void SetUp()
        {
            markdownToHtml = new Md();
        }
        
        [TestCase("", "")]
        [TestCase("text", "text")]
        [TestCase("text\n", "text\n")]
        [TestCase("text\ntext", "text\ntext")]
        public void MdRender_ShouldRenderCorrectly_WhenNoTagsInText(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }

        [TestCase("_text_", @"\<em>text\</em>")]
        [TestCase("__text__", @"\<strong>text\</strong>")]
        [TestCase("# text", @"\<h1>text\</h1>")]
        [TestCase("# text\n", "\\<h1>text\\</h1>\n")]
        [TestCase("# text\r\n", "\\<h1>text\\</h1>\r\n")]
        [TestCase("# text\n# text", "\\<h1>text\\</h1>\n\\<h1>text\\</h1>")]
        [TestCase("## text", @"\<h2>text\</h2>")]
        public void MdRender_ShouldRenderCorrectly_WhenTagContainsOnlySingleWord(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }
        
        [TestCase("+ text", "\\<ul>\n\\<li>text\\</li>\n\\</ul>")]
        [TestCase("+ listElement1\n"
                  + "+ listElement2\n"
                  + "+ listElement3", 
            "\\<ul>\n\\<li>listElement1\\</li>\n"
            + "\\<li>listElement2\\</li>\n"
            + "\\<li>listElement3\\</li>\n\\</ul>")]
        [TestCase("+ listElement1\r\n"
                  + "+ listElement2\r\n"
                  + "+ listElement3", 
            "\\<ul>\n\\<li>listElement1\\</li>\r\n"
            + "\\<li>listElement2\\</li>\r\n"
            + "\\<li>listElement3\\</li>\n\\</ul>")]
        [TestCase("+ listElement\n"
                  + "notListElement\n"
                  + "+ otherListElement", 
            "\\<ul>\n\\<li>listElement\\</li>\n\\</ul>\n"
            + "notListElement\n"
            + "\\<ul>\n\\<li>otherListElement\\</li>\n\\</ul>")]
        public void MdRender_ShouldRenderCorrectly_WhenBulletedListTag(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }

        [TestCase("__", "__")]
        [TestCase("____", "____")]
        public void MdRender_ShouldRenderCorrectly_WhenTagDontContainFilling(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }

        [TestCase("_text text_", @"\<em>text text\</em>")]
        [TestCase("__text text__", @"\<strong>text text\</strong>")]
        public void MdRender_ShouldRenderCorrectly_WhenTagContainsManyWords(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }
        
        [TestCase("__text text_", "__text text_")]
        [TestCase("_text text__", "_text text__")]
        [TestCase("_text text_____", "_text text_____")]
        [TestCase("__text text_____", "__text text_____")]
        [TestCase("__text _text__ text_", "__text _text__ text_")]
        [TestCase("_text __text__ text", @"_text \<strong>text\</strong> text")]
        [TestCase("_text __text__ text_", @"\<em>text __text__ text\</em>")]
        public void MdRender_ShouldRenderCorrectly_WhenUnpairedTagBorder(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }

        [TestCase("_begin_ middle end", @"\<em>begin\</em> middle end")]
        [TestCase("begin _middle_ end", @"begin \<em>middle\</em> end")]
        [TestCase("begin middle _end_", @"begin middle \<em>end\</em>")]
        public void MdRender_ShouldRenderCorrectly_WhenTagInDifferentPos(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }

        [TestCase("_begin_ _middle_ end", @"\<em>begin\</em> \<em>middle\</em> end")]
        [TestCase("_begin_ __middle__ end", @"\<em>begin\</em> \<strong>middle\</strong> end")]
        public void MdRender_ShouldRenderCorrectly_WhenManyTagsInText(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }
        
        [TestCase("__begin _middle_ end__", @"\<strong>begin \<em>middle\</em> end\</strong>")]
        [TestCase("___begin middle_ end__", @"\<strong>\<em>begin middle\</em> end\</strong>")]
        [TestCase("__begin _middle end___", @"\<strong>begin \<em>middle end\</em>\</strong>")]
        [TestCase("_begin __middle__ end_", @"\<em>begin __middle__ end\</em>")]
        [TestCase("# __begin _middle_ end__", @"\<h1>\<strong>begin \<em>middle\</em> end\</strong>\</h1>")]
        public void MdRender_ShouldRenderCorrectly_WhenTagNested(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }
        
        [TestCase("_1begin_ end", "_1begin_ end")]
        [TestCase("_begin2_ end", "_begin2_ end")]
        [TestCase("_begin_3 end", "_begin_3 end")]
        [TestCase("begin _4end_", "begin _4end_")]
        [TestCase("begin _end5_", "begin _end5_")]
        [TestCase("begin 6_end_", "begin 6_end_")]
        [TestCase("begin 7_8end9_1", "begin 7_8end9_1")]
        [TestCase("version3_18alpha_2", "version3_18alpha_2")]
        public void MdRender_ShouldRenderCorrectly_WhenStyleTagTouchDigit(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }
        
        [TestCase("be_gin_", @"be\<em>gin\</em>")]
        [TestCase("b_egi_n", @"b\<em>egi\</em>n")]
        [TestCase("_be_gin", @"\<em>be\</em>gin")]
        [TestCase("be_gin en_d", @"be_gin en_d")]
        public void MdRender_ShouldRenderCorrectly_WhenStyleTagCoverWordPart(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }
        
        [TestCase(@"text \ text", @"text \ text")]
        [TestCase(@"te\xt", @"te\xt")]
        [TestCase(@"te\\\\xt", @"te\\xt")]
        [TestCase(@"t\_e_x_t", @"t_e\<em>x\</em>t")]
        [TestCase(@"t\\_e_x_t", @"t\\<em>e\</em>x_t")]
        public void MdRender_ShouldRenderCorrectly_WhenTextContainsEscapedSymbols(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }
    }
}