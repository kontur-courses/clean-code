using NUnit.Framework;
using FluentAssertions;
using Markdown;


namespace MarkdownTests
{
    public class MdTest
    {
        private Md _md;

        [SetUp]
        public void Setup()
        {
            _md = new Md();
        }

        [TestCase("", "")]
        [TestCase("_text_", "<em>text</em>", TestName = "Курсив")]
        [TestCase("__text__", "<strong>text</strong>", TestName = "Полужирный тег")]
        [TestCase("#text", "<h1>text</h1>", TestName = "Заголовок")]
        public void Render_SimpleTests_ShouldBeExpected(string text, string exp)
        {
            var result = _md.Render(text);
            result.Should().BeEquivalentTo(exp);
        }


        [TestCase(@"# title __bold _italic_ bold__",
            @"<h1> title <strong>bold <em>italic</em> bold</strong></h1>",
            TestName = "Заголовок с тегами")]
        [TestCase("_нач_але, и в сер_еди_не, и в кон_це._",
            "<em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>",
            TestName = "Несколько тегов внутри слов")]
        [TestCase("__bold _italic_ bold__",
            "<strong>bold <em>italic</em> bold</strong>",
            TestName = "Курсив внутри полужирного тега")]
        [TestCase(@"text _italic __italic__ italic_ text.",
            @"text <em>italic __italic__ italic</em> text.",
            TestName = "Полужирный внутри курсива")]
        [TestCase(@"text \_text\_ text.", "text _text_ text.",
            TestName = "Одинарный слеш")]
        [TestCase(@"text \\_italic_ text", @"text \<em>italic</em> text",
            TestName = "Двойной слеш")]
        [TestCase(@"\\\_text_", "_text_",
            TestName = "Тройной слеш")]
        [TestCase(@"text_12_3",
            @"text_12_3",
            TestName = "Тег выделяющий цифры")]
        [TestCase("text_ text_ text", "text_ text_ text",
            TestName = "Пробел после тега")]
        [TestCase(@"text _text _text",
            @"text _text _text",
            TestName = "Пробел перед закрывающим тегом")]
        [TestCase(@"text __text _text__ text_ text",
            @"text __text _text__ text_ text",
            TestName = "Пересекающиеся теги")]
        public void Render_MultipleTags_ShouldBeExpected(string text, string exp)
        {
            var result = _md.Render(text);
            result.Should().BeEquivalentTo(exp);
        }


        [TestCase(@"te_xt te_xt", TestName = "Тег в разных словах")]
        [TestCase(@"text t\ext\ \text\", TestName = "Слеш без тегов")]
        public void Render_ShouldReturnText(string text)
        {
            var result = _md.Render(text);
            result.Should().BeEquivalentTo(text);
        }
    }
}