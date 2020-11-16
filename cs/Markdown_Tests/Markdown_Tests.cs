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

        [TestCase("_text_", @"\<em>text\</em>")]
        [TestCase("__text__", @"\<strong>text\</strong>")]
        [TestCase("# text", @"\<h1>text\</h1>")]
        public void MdRender_ShouldRenderCorrectly_WhenOnlySingleWordTag(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }

        [TestCase("_tex t_", @"\<em>tex t\</em>")]
        [TestCase("__tex t__", @"\<strong>tex t\</strong>")]
        [TestCase("# tex t", @"\<h1>tex t\</h1>")]
        public void MdRender_ShouldRenderCorrectly_WhenOnlyManyWordTag(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }

        [TestCase("_begin_ middle end", @"\<em>begin\</em> middle end")]
        [TestCase("begin _middle_ end", @"begin \<em>middle\</em> end")]
        [TestCase("begin middle _end_", @"begin middle \<em>end\</em>")]
        public void MdRender_ShouldRenderCorrectly_WhenSingleWordTagInText(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }

        [TestCase("_begin_ _middle_ end", @"\<em>begin\</em> \<em>middle\</em> end")]
        [TestCase("_begin __middle__ end_", @"\<em>begin \<strong>middle\</strong> end\</em>")]
        [TestCase("begin ___middle___ end", @"begin \<strong>\<em>middle\</em>\</strong> end")]
        public void MdRender_ShouldRenderCorrectly_WhenManyWordTagInText(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }
    }
}