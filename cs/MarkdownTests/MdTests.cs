using Markdown;
using NUnit.Framework;
using FluentAssertions;

namespace MarkdownTests
{
    [TestFixture]
    public class MdTests
    {
        [TestCase("_text_", "<em>text</em>")]
        [TestCase("__text__", "<strong>text</strong>")]
        [TestCase("#text\n", "<h1>text</h1>")]
        [TestCase("#\n", "#\n")]
        [TestCase("_", "_")]
        [TestCase("__", "__")]
        [TestCase("____", "____")]
        [TestCase("__text", "__text")]
        public void MdRender_ReturnParsedMarkup_WhenSimpleTags(string markupText, string expectedMarkup)
        {
            var md = Md.Render(markupText);

            md.Should().Be(expectedMarkup);
        }
        
        [TestCase("#text\n _text_", "<h1>text</h1> <em>text</em>")]
        [TestCase("#text\n __text__", "<h1>text</h1> <strong>text</strong>")]
        [TestCase("_text___text__", "<em>text</em><strong>text</strong>")]
        [TestCase("_text___text__#text\n", "<em>text</em><strong>text</strong><h1>text</h1>")]
        public void MdRender_ReturnParsedMarkup_WhenComplexTags(string markupText, string expectedMarkdown)
        {
            var md = Md.Render(markupText);

            md.Should().Be(expectedMarkdown);
        }
    }
}