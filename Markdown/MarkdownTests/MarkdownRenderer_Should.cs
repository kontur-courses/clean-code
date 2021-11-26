using FluentAssertions;
using Markdown;
using Markdown.Converters;
using Markdown.Factories.Html;
using Markdown.Factories.Markdown;
using Markdown.Parsers;
using Markdown.Tokenizer;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownRenderer_Should
    {
        private IMarkdownRenderer renderer;

        [SetUp]
        public void SetUp()
        {
            var parser = new MarkdownParser();

            var tokenizer = new Tokenizer(new MarkdownTokenFactory());

            var converter = new ConverterMarkdownToHtml(new HtmlTokenFactory());

            renderer = new MarkdownRenderer(parser, tokenizer, converter);
        }

        [Test]
        [TestCase("_privet_", ExpectedResult = "<em>privet</em>", TestName = "Emphasized")]
        [TestCase("__privet__", ExpectedResult = "<strong>privet</strong>", TestName = "Strong")]
        [TestCase("#privet", ExpectedResult = "<h1>privet</h1>", TestName = "Paragraph")]
        [TestCase("__please_work_!!!__", ExpectedResult = "<strong>please<em>work</em>!!!</strong>",
            TestName = "Emphasized inside strong")]
        [TestCase("# Заголовок __с _разными_ символами__",
            ExpectedResult = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>",
            TestName = "Example from specification")]
        [TestCase("\\\\_is em_", ExpectedResult = "\\<em>is em</em>", TestName = "Escaped escape symbol")]
        [TestCase("Здесь сим\\волы экранирования\\ \\должны остаться.\\",
            ExpectedResult = "Здесь сим\\волы экранирования\\ \\должны остаться.\\",
            TestName = "Escape symbols from example")]
        public string Render(string markdown)
        {
            var render = renderer.Render(markdown);

            return render;
        }

        [Test]
        [TestCase("__please_doesn't work___", ExpectedResult = "__please_doesn't work___",
            TestName = "Collision")] // В спецификации про это ничего нет, так что пока что это работает так, это поведение можно легко изменить, если мерджить в <strong> с конца
        [TestCase("_html__doesn't works__maybe_", ExpectedResult = "<em>html__doesn't works__maybe</em>",
            TestName = "Strong inside emphasized")]
        [TestCase("\\_not em_", ExpectedResult = "_not em_", TestName = "One of tags is escaped")]
        [TestCase("w_e aren't e_m", ExpectedResult = "w_e aren't e_m", TestName = "Tags in different words")]
        [TestCase("here ____ nothing in tags", ExpectedResult = "here ____ nothing in tags",
            TestName = "Empty string inside strong tag")]
        [TestCase("here __ nothing", ExpectedResult = "here __ nothing",
            TestName = "Empty string inside single underline")]
        [TestCase("a1_2b_", ExpectedResult = "a1_2b_", TestName = "Tags in text with digits")]
        [TestCase("_ a_", ExpectedResult = "_ a_", TestName = "Space after opening tag")]
        [TestCase("_a _", ExpectedResult = "_a _", TestName = "Space before closing tag")]
        public string DoesNotRender(string markdown)
        {
            var render = renderer.Render(markdown);

            return render;
        }
    }
}