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
        [TestCase("## text", @"\<h2>text\</h2>")]
        public void MdRender_ShouldRenderCorrectly_WhenTagContainsOnlySingleWord(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }

        [TestCase("_text text_", @"\<em>text text\</em>")]
        [TestCase("__text text__", @"\<strong>text text\</strong>")]
        public void MdRender_ShouldRenderCorrectly_WhenTagContainsManyWords(string markdownText, string htmlText)
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
        
        [TestCase("_begin __middle__ end_", @"\<em>begin \<strong>middle\</strong> end\</em>")]
        [TestCase("# _begin __middle__ end_", @"\<h1>\<em>begin \<strong>middle\</strong> end\</em>\</h1>")]
        [TestCase("begin ___middle___ end", @"begin \<strong>\<em>middle\</em>\</strong> end")]
        [TestCase("# begin ___middle___ end", @"\<h1>begin \<strong>\<em>middle\</em>\</strong> end\</h1>")]
        public void MdRender_ShouldRenderCorrectly_WhenTagNested(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }
        
        [TestCase("begin middle _end", "begin middle _end")]
        [TestCase("begin __middle _end", "begin __middle _end")]
        [TestCase("begin middle _end\n", "begin middle _end\n")]
        [TestCase("begin __middle _end\n", "begin __middle _end\n")]
        public void MdRender_ShouldRenderCorrectly_WhenTagCancelInEndOfLine(string markdownText, string htmlText)
        {
            markdownToHtml.Render(markdownText).Should().Be(htmlText);
        }
    }
}