using System;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using FluentAssertions.Execution;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownTests
    {
        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        private Md md;

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
        [TestCase("эти _подчерки _не считаются_ окончанием выделения",
            "эти <em>подчерки _не считаются</em> окончанием выделения")]
        public void Render_ShouldBeCorrect_WhenItalic(string text, string html)
        {
            var result = md.Render(text);
            result.Should().Be(html);
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
        [TestCase("эти __подчерки _не считаются__ окончанием выделения",
            "эти <strong>подчерки _не считаются</strong> окончанием выделения")]
        public void Render_ShouldBeCorrect_WhenBold(string text, string html)
        {
            var result = md.Render(text);
            result.Should().Be(html);
        }

        [TestCase("# Заголовок __с _разными_ символами__",
            "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
        [TestCase("#Заголовок __с _разными_ символами__", "#Заголовок <strong>с <em>разными</em> символами</strong>")]
        [TestCase("# Заголовок\n_текст_", "<h1>Заголовок</h1>\n<em>текст</em>")]
        public void Render_ShouldBeCorrect_WhenTitle(string text, string html)
        {
            var result = md.Render(text);
            result.Should().Be(html);
        }

        [TestCase("__пересечения _двойных_ и однарных__", "<strong>пересечения <em>двойных</em> и однарных</strong>")]
        [TestCase("_пересечения __двойных__ и однарных_", "<em>пересечения __двойных__ и однарных</em>")]
        [TestCase("__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_")]
        [TestCase("_пересечения __одинарных_ и двойных__", "_пересечения __одинарных_ и двойных__")]
        public void Render_ShouldBeCorrect_WhenBoldAndItalicIntersect(string text, string html)
        {
            var result = md.Render(text);
            result.Should().Be(html);
        }

        [TestCase(@"\_text\_", @"_text_")]
        [TestCase(@"abc\def abcd\ \abcd dd.\", @"abc\def abcd\ \abcd dd.\")]
        [TestCase(@"\\_text_", @"\<em>text</em>")]
        [TestCase(@"\_text_", @"_text_")]
        [TestCase(@"\__text_", @"_<em>text</em>")]
        public void Render_ShouldBeCorrect_WhenShielding(string text, string html)
        {
            var result = md.Render(text);
            result.Should().Be(html);
        }

        [Test]
        public void Render_ShouldBeFast()
        {
            var sb = new StringBuilder("# Заголовок __с _разными_ символами__ \n");
            for (var i = 1; i < 100; i++)
                sb.Append("a_a_ b__a_a_b__  __a_a__a_a__a_a__a__a__a_a_a_a_a__a_a_a_a_a__a__a__a");
            using (new AssertionScope())
            {
                var text = sb.ToString();
                var lastTime = Time(() => md.Render(text));
                for (var i = 0; i < 5; i++)
                {
                    sb.Append(sb);
                    text = sb.ToString();
                    var newTime = Time(() => md.Render(text));
                    var delta = newTime / lastTime;
                    lastTime = newTime;
                    delta.Should().BeInRange(0.5, 2.5);
                }
            }
        }

        private double Time(Action action)
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }


        [TestCase(@"[example](http://example.com/)", "<a href=\"http://example.com/\">example</a>")]
        [TestCase(@"[example](http://example.com/", "[example](http://example.com/")]
        [TestCase(@"[example]()", "<a href=\"\">example</a>")]
        [TestCase(@"[]()", "<a href=\"\"></a>")]
        [TestCase(@"[[[example]]]()", "<a href=\"\">[[example]]</a>")]
        [TestCase(@"[[[_example_]]]()", "<a href=\"\">[[<em>example</em>]]</a>")]
        [TestCase(@"[[[__example__]]]()", "<a href=\"\">[[<strong>example</strong>]]</a>")]
        [TestCase(@"[_example __aaa_ bb__]()", "<a href=\"\">_example __aaa_ bb__</a>")]
        [TestCase(@"aa[_examp]le __aaa_ bb__()", "aa[_examp]le __aaa_ bb__()")]
        [TestCase(@"_[_examp]()", "<em>[</em>examp]()")]
        public void Render_ShouldBeCorrect_WhenLink(string text, string html)
        {
            var result = md.Render(text);
            result.Should().Be(html);
        }
    }
}