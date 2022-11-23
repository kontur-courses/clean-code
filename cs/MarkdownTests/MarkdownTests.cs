using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownTests
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [TestCase("123_    _123", "123_    _123")]
        [TestCase("__", "__")]
        [TestCase("_text_", "<em>text</em>")]
        [TestCase("_123_", "_123_")]
        [TestCase("abc_ _def", "abc_ _def")]
        [TestCase("_нач_але", "<em>нач</em>але")]
        [TestCase("сер_еди_не", "сер<em>еди</em>не")]
        [TestCase("кон_це._", "кон<em>це.</em>")]
        [TestCase("выделение в ра_зных сл_овах не работает", "выделение в ра_зных сл_овах не работает")]
        [TestCase("эти_ подчерки_ не считаются выделением", "эти_ подчерки_ не считаются выделением")]
        [TestCase("эти _подчерки _не считаются_ окончанием выделения", "эти <em>подчерки _не считаются</em> окончанием выделения")]
        public void Render_ShouldBeCorrect_WhenItalic(string text, string html)
        {
            var result = md.Render(text);
            result.Should().Be(html); ;
        }

        [TestCase("123__    __123", "123__    __123")]
        [TestCase("____", "____")]
        [TestCase("__text__", "<strong>text</strong>")]
        [TestCase("__123__", "__123__")]
        [TestCase("abc__ __def", "abc__ __def")]
        [TestCase("__нач__але", "<strong>нач</strong>але")]
        [TestCase("сер__еди__не", "сер<strong>еди</strong>не")]
        [TestCase("кон__це.__", "кон<strong>це.</strong>")]
        [TestCase("выделение в ра__зных сл__овах не работает", "выделение в ра__зных сл__овах не работает")]
        [TestCase("эти__ подчерки__ не считаются выделением", "эти__ подчерки__ не считаются выделением")]
        [TestCase("эти __подчерки _не считаются__ окончанием выделения", "эти <strong>подчерки _не считаются</strong> окончанием выделения")]
        public void Render_ShouldBeCorrect_WhenBold(string text, string html)
        {
            var result = md.Render(text);
            result.Should().Be(html); ;
        }

        [TestCase("# Заголовок __с _разными_ символами__", "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
        [TestCase("#Заголовок __с _разными_ символами__", "#Заголовок <strong>с <em>разными</em> символами</strong>")]
        [TestCase("# Заголовок\n_текст_", "<h1>Заголовок</h1>\n<em>текст</em>")]
        public void Render_ShouldBeCorrect_WhenTitle(string text, string html)
        {
            var result = md.Render(text);
            result.Should().Be(html); ;
        }

        [TestCase("__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_")]
        [TestCase("_пересечения __одинарных_ и двойных__", "_пересечения __одинарных_ и двойных__")]
        public void Render_ShouldBeCorrect_WhenBoldAndItalicIntersect(string text, string html)
        {
            var result = md.Render(text);
            result.Should().Be(html); ;
        }

        [TestCase(@"\_text\_", @"_text_")]
        [TestCase(@"abc\def abcd\ \abcd dd.\", @"abc\def abcd\ \abcd dd.\")]
        [TestCase(@"\\_text_", @"\<em>text</em>")]
        [TestCase(@"\_text_", @"_text_")]
        [TestCase(@"\__text_", @"_<em>text</em>")]
        public void Render_ShouldBeCorrect_WhenShielding(string text, string html)
        {
            var result = md.Render(text);
            result.Should().Be(html); ;
        }

        [TestCase(10_000, 3000)]
        public void Render_ShouldBeFast(int count, int maxMilliseconds)
        {
            var sb = new StringBuilder("# Заголовок __с _разными_ символами__ \n");
            for (int i = 1; i < count; i++)
            {
                sb.Append("a_a_ b__a_a_b__  __a_a__a_a__a_a__a__a__a_a_a_a_a__a_a_a_a_a__a__a__a");
            }

            var test = sb.ToString();
            var sw = Stopwatch.StartNew();
            md.Render(test);
            sw.Stop();
            sw.ElapsedMilliseconds.Should().BeLessOrEqualTo(maxMilliseconds);
        }
    }
}
