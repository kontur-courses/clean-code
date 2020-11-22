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
        [TestCase("#text\n", "#text\n")]
        [TestCase("# text\n", "<h1>text</h1>")]
        [TestCase("#", "#")]
        [TestCase("_", "_")]
        [TestCase("__", "__")]
        [TestCase("____", "____")]
        [TestCase("__text", "__text")]
        [TestCase("_text _", "_text _")]
        [TestCase("_ text_", "_ text_")]
        [TestCase("__ text__", "__ text__")]
        public void MdRender_ReturnParsedMarkup_WhenSimpleTags(string markupText, string expectedMarkup)
        {
            var md = Md.Render(markupText);

            md.Should().Be(expectedMarkup);
        }
        
        [TestCase("# text\n _text_", "<h1>text</h1> <em>text</em>")]
        [TestCase("# text\n __text__", "<h1>text</h1> <strong>text</strong>")]
        [TestCase("_text___text__", "<em>text</em><strong>text</strong>")]
        [TestCase("_text___text", "<em>text</em>__text")]
        [TestCase("_text_ _text", "<em>text</em> _text")]
        [TestCase("_text___text__#text", "<em>text</em><strong>text</strong>#text")]
        public void MdRender_ReturnParsedMarkup_WhenSequenceTags(string markupText, string expectedMarkdown)
        {
            var md = Md.Render(markupText);

            md.Should().Be(expectedMarkdown);
        }
        
        [TestCase("\\_text\\_", "_text_")]
        [TestCase("\\#text", "#text")]
        [TestCase("\\_\\_text\\_\\_", "__text__")]
        [TestCase("\\__text_\\_", "_<em>text</em>_")]
        [TestCase("__text\\_\\_", "__text__")]
        [TestCase("__text\\_\\___", "<strong>text__</strong>")]
        [TestCase("\\\\", "\\")]
        [TestCase("\\", "\\")]
        [TestCase("te\\xt", "te\\xt")]
        [TestCase("\\\\_text\\\\_", "\\<em>text\\</em>")]
        [TestCase("\\\\__text\\\\__", "\\<strong>text\\</strong>")]
        [TestCase("\\\\#text\\\\\n", "\\#text\\\n")]
        [TestCase("\\\\# text\\\\\n", "\\# text\\\n")]
        [TestCase("# text\\\\\n", "<h1>text\\</h1>")]
        [TestCase("_text\\__", "<em>text_</em>")]
        [TestCase(@"Здесь сим\волы экранирования\ \должны остаться.\",
            @"Здесь сим\волы экранирования\ \должны остаться.\")]
        public void MdRender_ReturnParsedMarkup_WhenTextContainShieldSymbols(string markupText, string expectedMarkdown)
        {
            var md = Md.Render(markupText);

            md.Should().Be(expectedMarkdown);
        }
        
        [TestCase("__some _beautiful_ text__", "<strong>some <em>beautiful</em> text</strong>")]
        [TestCase("# Заголовок __с _разными_ символами__\n", 
            "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
        [TestCase("__some # beautiful text__", "<strong>some # beautiful text</strong>")]
        public void MdRender_ReturnParsedMarkup_WhenInteractedTags(string markupText, string expectedMarkdown)
        {
            var md = Md.Render(markupText);

            md.Should().Be(expectedMarkdown);
        }
    }
}