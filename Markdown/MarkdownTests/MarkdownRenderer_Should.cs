using Markdown;
using Markdown.Converters;
using Markdown.Parsers;
using Markdown.Tokenizers;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownRenderer_Should
    {
        private IMarkdownRenderer renderer;

        [SetUp]
        public void Setup()
        {
            var parser = new MarkdownParser();
            var tokenizer = new Tokenizer();
            var converter = new TokensToHtmlConverter();
            renderer = new MarkdownRenderer(parser, tokenizer, converter);
        }

        [Parallelizable]
        [TestCase("# privet", ExpectedResult = "<h1>privet</h1>", TestName = "Simple")]
        [TestCase("# Заголовок __с _разными_ символами__",
            ExpectedResult = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>",
            TestName = "Example from specification")]
        public string RenderParagraph(string markdown)
        {
            var result = renderer.Render(markdown);

            return result;
        }

        [Parallelizable]
        [TestCase("_privet_", ExpectedResult = "<em>privet</em>", TestName = "Simple")]
        [TestCase("__please_work___", ExpectedResult = "<strong>please<em>work</em></strong>",
            TestName = "Italic inside bold")]
        [TestCase("_a_123a_", ExpectedResult = "<em>a_123a</em>")]
        [TestCase("a_a a_a", ExpectedResult = "a_a a_a")]
        [TestCase("_a_a", ExpectedResult = "<em>a</em>a")]
        [TestCase("_a a_a", ExpectedResult = "_a a_a")]
        [TestCase("a_a a_", ExpectedResult = "a_a a_")]
        [TestCase("_a__a_b__", ExpectedResult = "_a__a_b__")]
        public string RenderItalic(string markdown)
        {
            var result = renderer.Render(markdown);

            return result;
        }

        [Parallelizable]
        [TestCase("__privet__", ExpectedResult = "<strong>privet</strong>", TestName = "Simple")]
        [TestCase("_it__doesnt__work_", ExpectedResult = "<em>it__doesnt__work</em>")]
        [TestCase("a__a a__a", ExpectedResult = "a__a a__a")]
        [TestCase("__a a__a", ExpectedResult = "__a a__a")]
        [TestCase("a__a a__", ExpectedResult = "a__a a__")]
        public string RenderBold(string markdown)
        {
            var result = renderer.Render(markdown);

            return result;
        }

        [Parallelizable]
        [TestCase("Здесь сим\\волы экранирования\\ \\должны остаться.\\",
            ExpectedResult = "Здесь сим\\волы экранирования\\ \\должны остаться.\\",
            TestName = "Example from specification")]
        public string RenderEscape(string markdown)
        {
            var result = renderer.Render(markdown);

            return result;
        }
    }
}