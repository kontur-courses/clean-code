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

        [Test]
        public void Render_MultipleParagraphs_RendersCorrectly()
        {
            Assert.That(md.Render("Первый абзац\nВторой абзац"),
                Is.EqualTo("<p>Первый абзац</p><p>Второй абзац</p>"));
        }

        [TestCase("_Курсив_", "<p><em>Курсив</em></p>", TestName = "Valid Italic")]
        [TestCase("__Жирный__", "<p><strong>Жирный</strong></p>", TestName = "Valid Strong")]
        [TestCase("[Ссылка](http://test.com)", "<p><a href=\"http://test.com\">Ссылка</a></p>", TestName = "Valid Link")]
        public void Render_BasicTags_ReturnsHtml(string input, string expected)
        {
            Assert.That(md.Render(input), Is.EqualTo(expected));
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

        [TestCase("# ", "<h1></h1>", TestName = "Trailing space in heading is not trimmed")]
        [TestCase("#", "<p>#</p>", TestName = "Heading without space should be paragraph")]
        [TestCase("#Текст", "<p>#Текст</p>", TestName = "Hash without space should be paragraph")]
        public void Render_InvalidHeadingFormat_ReturnsParagraph(string input, string expected)
        {
            Assert.That(md.Render(input), Is.EqualTo(expected));
        }

        [Test]
        public void Render_StrongWithNestedItalic_RendersCorrectly()
        {
            Assert.That(md.Render("__Жирный _и курсив___"),
                Is.EqualTo("<p><strong>Жирный <em>и курсив</em></strong></p>"));
        }

        [Test]
        public void Render_ItalicWithNestedStrong_DoesNotFormatStrong()
        {
            Assert.That(md.Render("_текст __жирный__ текст_"),
                Is.EqualTo("<p><em>текст __жирный__ текст</em></p>"));
        }
        [TestCase("[текст](", "<p>[текст](</p>", TestName = "Unclosed link parenthesis is not formatted")]
        [TestCase("_текст", "<p>_текст</p>", TestName = "Unpaired opening underscore")]
        [TestCase("текст_", "<p>текст_</p>", TestName = "Unpaired closing underscore")]
        [TestCase("__текст", "<p>__текст</p>", TestName = "Unpaired opening double underscore")]
        [TestCase("текст__", "<p>текст__</p>", TestName = "Unpaired closing double underscore")]
        [TestCase("____", "<p>____</p>", TestName = "Empty underscores are ignored")]
        [TestCase("_ текст_", "<p>_ текст_</p>", TestName = "Space after opening underscore is ignored")]
        [TestCase("_текст _", "<p>_текст _</p>", TestName = "Space before closing underscore is ignored")]
        [TestCase("_текст __жирный_ текст__", "<p>_текст __жирный_ текст__</p>", TestName = "Intersecting tags are ignored")]
        [TestCase("_текст__жирный__", "<p>_текст<strong>жирный</strong></p>", TestName = "Italic without closing underscore")]
        [TestCase("ра_зных сл_овах", "<p>ра_зных сл_овах</p>", TestName = "Underscores between different words are ignored italic")]
        [TestCase("ра__зных сл__овах", "<p>ра__зных сл__овах</p>", TestName = "Underscores between different words are ignored strong")]
        public void Render_InvalidUnderscores_NotFormatted(string input, string expected)
        {
            Assert.That(md.Render(input), Is.EqualTo(expected));
        }

        [TestCase("ч_12_3", "<p>ч_12_3</p>", TestName = "Italic inside digits")]
        [TestCase("12_текст_", "<p>12_текст_</p>", TestName = "Italic starts after digit")]
        [TestCase("_текст_12", "<p>_текст_12</p>", TestName = "Italic ends before digit")]
        [TestCase("ч__12__3", "<p>ч__12__3</p>", TestName = "Strong inside digits")]
        [TestCase("12__текст__", "<p>12__текст__</p>", TestName = "Strong starts after digit")]
        public void Render_UnderscoresWithDigits_NotFormatted(string input, string expected)
        {
            Assert.That(md.Render(input), Is.EqualTo(expected));
        }

        [TestCase(@"\_Курсив\_", "<p>_Курсив_</p>", TestName = "Escaped Italic")]
        [TestCase(@"\# Заголовок", "<p># Заголовок</p>", TestName = "Escaped Heading")]
        [TestCase(@"\(Ссылка\)", "<p>(Ссылка)</p>", TestName = "Escaped parentheses are ignored")]
        public void Render_EscapedSymbols_IgnoresMarkup(string input, string expected)
        {
            Assert.That(md.Render(input), Is.EqualTo(expected));
        }

        [TestCase(@"_Курсив\_", "<p>_Курсив_</p>", TestName = "Escaped closing italic underscore is not parsed")]
        [TestCase(@"__Жирный\__", "<p>__Жирный__</p>", TestName = "Escaped closing strong underscore is not parsed")]
        [TestCase(@"[Ссылка\](http://test.com)", "<p>[Ссылка](http://test.com)</p>", TestName = "Escaped link bracket is not parsed")]
        public void Render_EscapedClosingSymbols_IgnoresMarkup(string input, string expected)
        {
            Assert.That(md.Render(input), Is.EqualTo(expected));
        }

        [TestCase("<script>alert('XSS')</script>", "<p>&lt;script&gt;alert('XSS')&lt;/script&gt;</p>", TestName = "Script tags are escaped")]
        [TestCase("Текст <b>жирный</b>", "<p>Текст &lt;b&gt;жирный&lt;/b&gt;</p>", TestName = "HTML tags are escaped")]
        [TestCase("Кот & собака", "<p>Кот &amp; собака</p>", TestName = "Ampersand is escaped")]
        public void Render_HtmlTagsInText_AreEscaped(string input, string expected)
        {
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

        [Test]
        public void Render_ManyOpenBrackets_CompletesFast()
        {
            var longText = new string('[', 100_000);
            var stopwatch = Stopwatch.StartNew();

            md.Render(longText);

            stopwatch.Stop();
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(200));
        }

        [TestCase(@"[текст](url\))", "<p><a href=\"url)\">текст</a></p>", TestName = "Escaped closing parenthesis in URL")]
        [TestCase(@"[текст](url\\)", "<p><a href=\"url\\\">текст</a></p>", TestName = "Even backslashes before closing parenthesis")]
        [TestCase(@"[текст](url/path)", "<p><a href=\"url/path\">текст</a></p>", TestName = "Normal slash in URL")]
        [TestCase(@"[ссылка](https://example.com/path\_part)", "<p><a href=\"https://example.com/path_part\">ссылка</a></p>", TestName = "Escaped underscore in URL")]
        [TestCase(@"[ссылка](https://example.com/path\\part)", "<p><a href=\"https://example.com/path\\part\">ссылка</a></p>", TestName = "Escaped backslash in URL")]
        public void Render_UrlsWithSpecialCharacters_RendersCorrectly(string input, string expected)
        {
            Assert.That(md.Render(input), Is.EqualTo(expected));
        }

        [TestCase(@"\__текст_", "<p>_<em>текст</em></p>", TestName = "Escaped first underscore followed by italic")]
        [TestCase(@"*текст \_ хвост*", "<p>*текст _ хвост*</p>", TestName = "Escaped underscore inside asterisk text")]
        [TestCase(@"_текст \_ хвост_", "<p><em>текст _ хвост</em></p>", TestName = "Escaped underscore inside italic")]
        [TestCase(@"_текст \\_ хвост_", "<p><em>текст \\</em> хвост_</p>", TestName = "Even backslashes do not escape underscore")]
        public void Render_EscapedCharactersInsideTags_RendersCorrectly(string input, string expected)
        {
            Assert.That(md.Render(input), Is.EqualTo(expected));
        }

        [TestCase("__пересечения _двойных__ и одинарных_", "<p>__пересечения _двойных__ и одинарных_</p>", TestName = "Intersecting strong and italic")]
        [TestCase("_текст _ещё_", "<p><em>текст _ещё</em></p>", TestName = "Nested italic without closing first")]
        [TestCase("__текст __ещё__", "<p><strong>текст __ещё</strong></p>", TestName = "Nested strong without closing first")]
        public void Render_IntersectingTags_NotFormatted(string input, string expected)
        {
            Assert.That(md.Render(input), Is.EqualTo(expected));
        }
    }
}