using NUnit.Framework;
using System.Diagnostics;

namespace Markdown.Tests
{
    [TestFixture]
    public class MdTests
    {
        private Md md = null!;

        [SetUp]
        public void Setup() => md = new Md();

        [Test]
        public void Render_EmptyString_ReturnsEmptyString()
        {
            Assert.That(md.Render(""), Is.EqualTo(""));
        }

        [Test]
        public void Render_NullString_ReturnsEmptyString()
        {
            Assert.That(md.Render(null), Is.EqualTo(""));
        }

        [Test]
        public void Render_SimpleText_ReturnsParagraph()
        {
            Assert.That(md.Render("Просто текст"), Is.EqualTo("<p>Просто текст</p>"));
        }

        [TestCase("_Курсив_", "<p><em>Курсив</em></p>", TestName = "Valid Italic")]
        [TestCase("__Жирный__", "<p><strong>Жирный</strong></p>", TestName = "Valid Strong")]
        [TestCase("[Ссылка](http://test.com)", "<p><a href=\"http://test.com\">Ссылка</a></p>", TestName = "Valid Link")]
        public void Render_BasicTags_ReturnsHtml(string input, string expected)
        {
            Assert.That(md.Render(input), Is.EqualTo(expected));
        }

        [Test]
        public void Render_Heading_ReturnsH1Tag()
        {
            Assert.That(md.Render("# Заголовок"), Is.EqualTo("<h1>Заголовок</h1>"));
        }

        [TestCase("# Заголовок", "<h1>Заголовок</h1>", TestName = "Simple heading")]
        [TestCase("# Заголовок с _курсивом_", "<h1>Заголовок с <em>курсивом</em></h1>", TestName = "Heading with italic")]
        [TestCase("# Заголовок с __жирным__", "<h1>Заголовок с <strong>жирным</strong></h1>", TestName = "Heading with strong")]
        [TestCase("# Заголовок с [ссылкой](http://test.com)", "<h1>Заголовок с <a href=\"http://test.com\">ссылкой</a></h1>", TestName = "Heading with link")]

        [TestCase("# __Жирный _и курсив_ в заголовке__", "<h1><strong>Жирный <em>и курсив</em> в заголовке</strong></h1>", TestName = "Heading with mixed inline tags")]
        public void Render_Heading_ReturnsH1TagWithInlineTags(string input, string expected)
        {
            Assert.That(md.Render(input), Is.EqualTo(expected));
        }

        [TestCase(@"\_Курсив\_", "<p>_Курсив_</p>", TestName = "Escaped Italic")]
        [TestCase(@"\# Заголовок", "<p># Заголовок</p>", TestName = "Escaped Heading")]
        public void Render_EscapedSymbols_IgnoresMarkup(string input, string expected)
        {
            Assert.That(md.Render(input), Is.EqualTo(expected));
        }

        [Test]
        public void Render_StrongWithNestedItalic_RendersCorrectly()
        {
            Assert.That(md.Render("__Жирный _и курсив___"), Is.EqualTo("<p><strong>Жирный <em>и курсив</em></strong></p>"));
        }

        [Test]
        public void Render_ItalicWithNestedStrong_DoesNotFormatStrong()
        {
            Assert.That(md.Render("_текст __жирный__ текст_"), Is.EqualTo("<p><em>текст __жирный__ текст</em></p>"));
        }

        [TestCase("_текст", "<p>_текст</p>", TestName = "Unpaired opening underscore")]
        [TestCase("текст_", "<p>текст_</p>", TestName = "Unpaired closing underscore")]
        public void Render_UnpairedUnderscores_NotFormatted(string input, string expected)
        {
            Assert.That(md.Render(input), Is.EqualTo(expected));
        }


        [TestCase("ч_12_3", "<p>ч_12_3</p>", TestName = "Underscores inside digits are ignored")]
        [TestCase("____", "<p>____</p>", TestName = "Empty underscores are ignored")]
        public void Render_InvalidContextUnderscores_NotFormatted(string input, string expected)
        {
            Assert.That(md.Render(input), Is.EqualTo(expected));
        }


        [TestCase("_ текст_", "<p>_ текст_</p>", TestName = "Space after opening underscore is ignored")]
        [TestCase("_текст _", "<p>_текст _</p>", TestName = "Space before closing underscore is ignored")]
        public void Render_UnderscoresWithSpaces_NotFormatted(string input, string expected)
        {
            Assert.That(md.Render(input), Is.EqualTo(expected));
        }

        [Test]
        public void Render_MultipleParagraphs_RendersCorrectly()
        {
            var input = "Первый абзац\nВторой абзац";
            var expected = "<p>Первый абзац</p><p>Второй абзац</p>";

            Assert.That(md.Render(input), Is.EqualTo(expected));
        }

        [Test]
        public void Render_OneMillionChars_CompletesUnder200Ms()
        {
            var longText = new string('a', 1_000_000);
            var stopwatch = Stopwatch.StartNew();

            md.Render(longText);

            stopwatch.Stop();
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(200));
        }
    }
}