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
        // На прошлом ревью это работало наоборот, но в спецификации указано, что должен работать именно "# " 
        [TestCase("#privet", ExpectedResult = "#privet", TestName = "Doesn't render header if not space after #")]
        [TestCase("privet# ", ExpectedResult = "privet# ",
            TestName = "Doesn't render if header is not at line start")]
        [TestCase("# a\n# b\n# c", ExpectedResult = "<h1>a</h1>\n<h1>b</h1>\n<h1>c</h1>", TestName = "Multiple lines")]
        public string RenderHeader(string markdown)
        {
            var result = renderer.Render(markdown);

            return result;
        }

        [Parallelizable]
        [TestCase("_privet_", ExpectedResult = "<em>privet</em>", TestName = "Simple")]
        [TestCase("_a_123a_", ExpectedResult = "<em>a_123a</em>",
            TestName = "Doesn't pairing with tag in word with digits")]
        [TestCase("a_a a_a", ExpectedResult = "a_a a_a", TestName = "Doesn't pairing tags in different words")]
        [TestCase("_a_a a_a_a a_a_", ExpectedResult = "<em>a</em>a a<em>a</em>a a<em>a</em>",
            TestName = "Emphasizing part of word")]
        [TestCase("_a a_a", ExpectedResult = "_a a_a",
            TestName = "When tag in word doesn't pairing with tag in another word")]
        [TestCase("a_a a_", ExpectedResult = "a_a a_",
            TestName = "When tag not in word doesn't pairing with tag in another word")]
        [TestCase("_a_a_", ExpectedResult = "<em>a</em>a_", TestName = "Unpaired stays underline")]
        [TestCase("_a\na_", ExpectedResult = "_a\na_", TestName = "Doesn't pairing tags on different lines")]
        [TestCase("_ a_", ExpectedResult = "_ a_", TestName = "Doesn't pairing if space after opening tag")]
        [TestCase("_a _", ExpectedResult = "_a _", TestName = "Doesn't pairing if space before closing tag")]
        public string RenderEmphasized(string markdown)
        {
            var result = renderer.Render(markdown);

            return result;
        }

        [Parallelizable]
        [TestCase("__privet__", ExpectedResult = "<strong>privet</strong>", TestName = "Simple")]
        [TestCase("__a__123a__", ExpectedResult = "<strong>a__123a</strong>",
            TestName = "Doesn't pairing with tag in word with digits")]
        [TestCase("a__a a__a", ExpectedResult = "a__a a__a", TestName = "Doesn't pairing tags in different words")]
        [TestCase("__a__a a__a__a a__a__",
            ExpectedResult = "<strong>a</strong>a a<strong>a</strong>a a<strong>a</strong>",
            TestName = "Strong part of word")]
        [TestCase("__a a__a", ExpectedResult = "__a a__a",
            TestName = "When tag in word doesn't pairing with tag in another word")]
        [TestCase("a__a a__", ExpectedResult = "a__a a__",
            TestName = "When tag not in word doesn't pairing with tag in another word")]
        [TestCase("__a__a__", ExpectedResult = "<strong>a</strong>a__", TestName = "Unpaired stays underlines")]
        [TestCase("__a\na__", ExpectedResult = "__a\na__", TestName = "Doesn't pairing tags on different lines")]
        [TestCase("__ a__", ExpectedResult = "__ a__", TestName = "Doesn't pairing if space after opening tag")]
        [TestCase("__a __", ExpectedResult = "__a __", TestName = "Doesn't pairing if space before closing tag")]
        [TestCase("____", ExpectedResult = "____",
            TestName = "Stays underlines if empty string beetween opening and closing tag")]
        public string RenderStrong(string markdown)
        {
            var result = renderer.Render(markdown);

            return result;
        }

        [Parallelizable]
        [TestCase("Здесь сим\\волы экранирования\\ \\должны остаться.\\",
            ExpectedResult = "Здесь сим\\волы экранирования\\ \\должны остаться.\\",
            TestName = "Example from specification")]
        [TestCase("\\_a_", ExpectedResult = "_a_", TestName = "Doesn't render escaped opening em tag")]
        [TestCase("\\__a__", ExpectedResult = "__a__", TestName = "Doesn't render escaped opening strong tag")]
        [TestCase("_a\\_", ExpectedResult = "_a_", TestName = "Doesn't render escaped closing em tag")]
        [TestCase("__a\\__", ExpectedResult = "__a__", TestName = "Doesn't render escaped closing strong tag")]
        public string RenderEscape(string markdown)
        {
            var result = renderer.Render(markdown);

            return result;
        }
    }
}