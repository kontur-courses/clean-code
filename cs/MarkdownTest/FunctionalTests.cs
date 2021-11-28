using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    public class FunctionalTests
    {
        private Md markdown;
        
        [SetUp]
        public void SetUp()
        {
            var creator = new TokenCreator();
            var simplifier = new Reducer();
            var parser = new TokenParser();
            var render = new Renderer();
            markdown = new Md(creator, simplifier, parser, render);
        }

        [TestCase("\\", ExpectedResult = "\\", TestName = "Only escape symbol")]
        [TestCase("\\# abc", ExpectedResult = "# abc", TestName = "Escape header")]
        [TestCase("\\_abc_", ExpectedResult = "_abc_", TestName = "Shielding italics")]
        [TestCase("\\__abc__", ExpectedResult = "__abc__", TestName = "Escape strong")]
        [TestCase("\\a\\b\\c", ExpectedResult = "\\a\\b\\c", TestName = "Escape text")]
        [TestCase("\\\\", ExpectedResult = "\\", TestName = "Escape shield escape itself")]
        public string Render_ShouldWorkCorrectly_WhenEscape(string text)
        {
            return markdown.Render(text);
        }

        [TestCase("_t_", ExpectedResult = "<em>t</em>", TestName = "Cover text")]
        [TestCase("_a_a", ExpectedResult = "<em>a</em>a", TestName = "Cover Word part")]
        [TestCase("_1_a", ExpectedResult = "_1_a", TestName = "Numbers Nearby")]
        public string Render_ShouldWorkCorrectly_WhenItalics(string text)
        {
            return markdown.Render(text);
        }
        
        [TestCase("__t__", ExpectedResult = "<strong>t</strong>", TestName = "Cover text")]
        [TestCase("__a__a", ExpectedResult = "<strong>a</strong>a", TestName = "Cover Word part")]
        [TestCase("__1__a", ExpectedResult = "__1__a", TestName = "Numbers Nearby")]
        public string Render_ShouldWorkCorrectly_WhenStrong(string text)
        {
            return markdown.Render(text);
        }

        [TestCase("__a_text__c_", ExpectedResult = "__a_text__c_", TestName = "Italics intersect strong")]
        [TestCase("_a__text_c__", ExpectedResult = "_a__text_c__", TestName = "Strong intersect italics")]
        [TestCase("Внутри __двойного выделения _одинарное_ тоже__ работает", 
            ExpectedResult = "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает")]
        [TestCase("Внутри _одинарного __двойное__ не_ работает", 
            ExpectedResult = "Внутри _одинарного __двойное__ не_ работает")]
        public string Render_ShouldWorkCorrectly_ShouldWorkWith(string text)
        {
            return markdown.Render(text);
        }
        
        [TestCase("# text", ExpectedResult = "<h1>text</h1>", TestName = "Header1 and text")]
        [TestCase("# # text", ExpectedResult = "<h1># text</h1>", TestName = "Two header1")]
        [TestCase("text # text", ExpectedResult = "text # text", TestName = "Header1 not at the beginning")]
        [TestCase("# Заголовок __с _разными_ симоволами__", 
            ExpectedResult = "<h1>Заголовок <strong>с <em>разными</em> симоволами</strong></h1>")]
        public string Render_ShouldWorkCorrectly_WhenHeader(string text)
        {
            return markdown.Render(text);
        }

        [TestCase("[спецификации](MarkdownSpec.md)", 
            ExpectedResult = "спецификации href=\"MarkdownSpec.md\"", TestName = "basic link")]
        [TestCase("[__спецификации__](MarkdownSpec.md)", 
            ExpectedResult = "<strong>спецификации</strong> href=\"MarkdownSpec.md\"", TestName = "Strong link")]
        public string Render_ShouldWorkCorrectly_WhenLink(string text)
        {
            return markdown.Render(text);
        }
    }
}