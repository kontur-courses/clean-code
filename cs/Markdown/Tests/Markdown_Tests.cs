using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Markdown_Tests
    {
        [TestCase("# Heading", "<h1>Heading</h1>")]
        [TestCase("# Heading _With em text_", "<h1>Heading <em>With em text</em></h1>")]
        [TestCase("# Heading __With strong text__", "<h1>Heading <strong>With strong text</strong></h1>")]
        [TestCase("# Heading __In strong _em text_ and more__", "<h1>Heading <strong>In strong <em>em text</em> and more</strong></h1>")]
        [TestCase("# Heading ___Only strong and em text___", "<h1>Heading ___Only strong and em text___</h1>")]
        [TestCase("# Heading _In em not __strong text__ exists_", "<h1>Heading <em>In em not __strong text__ exists</em></h1>")]
        public void MarkdownHeading_Render_ReturnsExpectedResult(string input, string expected)
        {
            Md.Render(input).Should().Be(expected);
        }

        [TestCase("abc", "<p>abc</p>")]
        [TestCase("abc _em_ __strong__", "<p>abc <em>em</em> <strong>strong</strong></p>")]
        [TestCase("Paragraph __In strong _em text_ and more__", "<p>Paragraph <strong>In strong <em>em text</em> and more</strong></p>")]
        [TestCase("___Only strong and em text___", "<p>___Only strong and em text___</p>")]
        [TestCase("_In em not __strong text__ exists_", "<p><em>In em not __strong text__ exists</em></p>")]
        [TestCase("Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._", "<p>Однако выделять часть слова они могут: и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em></p>")]
        [TestCase("В то же время выделение в ра_зных сл_овах не работает.", "<p>В то же время выделение в ра_зных сл_овах не работает.</p>")]
        [TestCase("В ра__зных сл__овах не работает.", "<p>В ра__зных сл__овах не работает.</p>")]

        [TestCase("Подчерки внутри текста c цифрами_12_3 не считаются выделением", "<p>Подчерки внутри текста c цифрами_12_3 не считаются выделением</p>")]
        [TestCase("_123_", "<p><em>123</em></p>")]
        [TestCase("_123a123_", "<p><em>123a123</em></p>")]
        [TestCase("_v2_", "<p><em>v2</em></p>")]
        [TestCase("a_12_b", "<p>a_12_b</p>")]
        [TestCase("__123__", "<p><strong>123</strong></p>")]
        [TestCase("__123a123__", "<p><strong>123a123</strong></p>")]
        [TestCase("a__123__b", "<p>a__123__b</p>")]

        [TestCase("__Непарные_ символы в рамках одного абзаца не считаются выделением.", "<p>__Непарные_ символы в рамках одного абзаца не считаются выделением.</p>")]
        [TestCase("Иначе эти_ подчерки_ не считаются выделением", "<p>Иначе эти_ подчерки_ не считаются выделением</p>")]
        [TestCase("Иначе эти _подчерки _не считаются", "<p>Иначе эти _подчерки _не считаются</p>")]
        [TestCase("В случае __пересечения _двойных__ и одинарных_ подчерков", "<p>В случае __пересечения _двойных__ и одинарных_ подчерков</p>")]
        [TestCase("__ab_c_d__", "<p><strong>ab<em>c</em>d</strong></p>")]
        [TestCase("__a_b__c_", "<p>__a_b__c_</p>")]

        public void MarkdownParagraph_Render_ReturnsExpectedResult(string input, string expected)
        {
            Md.Render(input).Should().Be(expected);
        }

        [TestCase(@"\_вот это НЕ будет выделено Em тегом_", @"<p>_вот это НЕ будет выделено Em тегом_</p>")]
        [TestCase(@"\\_вот это будет выделено Em тегом_", @"<p>\<em>вот это будет выделено Em тегом</em></p>")]
        [TestCase(@"\_\_вот это НЕ будет выделено Strong тегом__", @"<p>__вот это НЕ будет выделено Strong тегом__</p>")]
        [TestCase(@"\__вот это будет выделено Em, а не Strong тегом_\_", @"<p>_<em>вот это будет выделено Em, а не Strong тегом</em>_</p>")]
        public void Markdown_RenderScreeningSymbols_ReturnsExpectedResult(string input, string expected)
        {
            Md.Render(input).Should().Be(expected);
        }

        [TestCase(@"[abc](url)", @"<p><a href=""url"">abc</a></p>")]
        [TestCase(@"[_abc_](url)", @"<p><a href=""url""><em>abc</em></a></p>")]
        [TestCase(@"[_abc_](u_r_l)", @"<p><a href=""u_r_l""><em>abc</em></a></p>")]
        [TestCase(@"[_abc_](url", @"<p>[<em>abc</em>](url</p>")]
        [TestCase(@"\[_abc_](url)", @"<p>[<em>abc</em>](url)</p>")]
        public void Markdown_RenderLinks_ReturnsExpectedResult(string input, string expected)
        {
            Md.Render(input).Should().Be(expected);
        }


        [Test]
        public void Markdown_Render_ReturnsExpectedResultInRealTests()
        {
            string input = Path.Combine(AppContext.BaseDirectory, "RealFileTests", "methodical_text.txt");
            string expected = Path.Combine(AppContext.BaseDirectory, "RealFileTests", "methodical_text_html.txt");
            Md.Render(File.ReadAllText(input).ToString()!).Should().Be(File.ReadAllText(expected).Replace("\r\n", "\n"));
        }
    }
}
