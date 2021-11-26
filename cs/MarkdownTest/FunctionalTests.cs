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
        [TestCase("_a__text__c_", ExpectedResult = "<em>a<strong>text</strong>c</em>", TestName = "Strong in italics")]
        public string Render_ShouldWorkCorrectly_WhenStrongAndItalics(string text)
        {
            return markdown.Render(text);
        }
        
        [TestCase("# text", ExpectedResult = "<h1>text</h1>", TestName = "Header1 and text")]
        [TestCase("# # text", ExpectedResult = "<h1># text</h1>", TestName = "Two header1")]
        [TestCase("text # text", ExpectedResult = "text # text", TestName = "Header1 not at the beginning")]
        public string Render_ShouldWorkCorrectly_WhenHeader(string text)
        {
            return markdown.Render(text);
        }
    }
}