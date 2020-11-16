using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using AutoFixture;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownTests
    {
        private Markdown.Markdown markdown;
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            markdown = new Markdown.Markdown();
        }

        [Test]
        [TestCase("single", ExpectedResult = "single", TestName = "No style")]
        [TestCase("_text_", ExpectedResult = "<em>text</em>", TestName = "Angled")]
        [TestCase("__text__", ExpectedResult = "<strong>text</strong>", TestName = "Bold")]
        [TestCase("# text", ExpectedResult = "<h1>text</h1>", TestName = "Header")]
        [TestCase("\\_text\\_", ExpectedResult = "_text_", TestName = "Escape symbol escapes style")]
        [TestCase("\\text\\", ExpectedResult = "\\text\\",
            TestName = "Escape symbol does not disappear when does not escape style")]
        [TestCase("__двойного выделения _одинарное_ тоже__",
            ExpectedResult = "<strong>двойного выделения <em>одинарное</em> тоже</strong>",
            TestName = "Angled inside bold")]
        [TestCase("_одинарного __двойное__ не_", ExpectedResult = "<em>одинарного __двойное__ не</em>",
            TestName = "Bold inside angled, angled style only")]
        [TestCase("цифрами_12_3", ExpectedResult = "цифрами_12_3", TestName = "Underscored numbers are underscores")]
        [TestCase("_нач_але", ExpectedResult = "<em>нач</em>але", TestName = "Underscore start middle")]
        [TestCase("сер_еди_не", ExpectedResult = "сер<em>еди</em>не", TestName = "Underscore middle middle")]
        [TestCase("кон_це._", ExpectedResult = "кон<em>це.</em>", TestName = "Underscore middle end")]
        [TestCase("ра_зных сл_овах", ExpectedResult = "ра_зных сл_овах",
            TestName = "Underscore in middle of different words does not work")]
        [TestCase("__Непарные_", ExpectedResult = "__Непарные_",
            TestName = "Unclosed style in paragraph double single")]
        [TestCase("__Непарные", ExpectedResult = "__Непарные", TestName = "Unclosed style in paragraph double")]
        [TestCase("_Непарные", ExpectedResult = "_Непарные", TestName = "Unclosed style in paragraph single")]
        [TestCase("эти_ подчерки_ не считаются выделением", ExpectedResult = "эти_ подчерки_ не считаются выделением",
            TestName = "Not a style start when whitespace after style symbol")]
        [TestCase("эти _подчерки _не считаются_ окончанием выделения",
            ExpectedResult = "эти _подчерки <em>не считаются</em> окончанием выделения",
            TestName = "Not a style end when whitespace before style symbol")]
        [TestCase("__пересечения _двойных__ и одинарных_", ExpectedResult = "__пересечения _двойных__ и одинарных_",
            TestName = "No style when styles intersect")]
        [TestCase("____", ExpectedResult = "____", TestName = "Empty text with style does not change")]
        [TestCase("# Заголовок __с _разными_ символами__",
            ExpectedResult = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>",
            TestName = "Paragraph")]
        [TestCase("_с_ _символами_", ExpectedResult = "<em>с</em> <em>символами</em>", TestName = "Two underscores")]
        [TestCase("_a_ _b_  _c_", ExpectedResult = "<em>a</em> <em>b</em>  <em>c</em>", TestName = "Three underscores")]
        [TestCase("![описание](https://picture1)", ExpectedResult = "<img src=\"https://picture1\" alt=\"описание\">",
            TestName = "Picture")]
        [TestCase("[имя](https://link)", ExpectedResult = "<a href=\"https://link\">имя</a>", TestName = "Link")]
        public string Markdown_Render(string text)
        {
            return markdown.Render(text);
        }

        [Test]
        public void Render_IsLinearComplexity()
        {
            const int blockSize = 1000;
            const int count = 100;
            var textData = new List<string>();
            for (var i = 1; i <= count; i++)
                textData.Add(RandomString(blockSize * i));
            var times = new List<(int, long)>();
            var stopwatch = new Stopwatch();

            foreach (var text in textData)
            {
                stopwatch.Start();
                var _ = markdown.Render(text);
                stopwatch.Stop();
                Console.WriteLine("Elapsed={0}", stopwatch.ElapsedMilliseconds);
                times.Add((text.Length, stopwatch.ElapsedMilliseconds));
            }

            var timeVectors = times.Select(pair => new Vector2(pair.Item1, pair.Item2)).ToList();
            var angles = new List<double>();
            foreach (var vector1 in timeVectors)
            {
                foreach (var vector2 in timeVectors.Where(vector => vector != vector1))
                {
                    var deviation = vector1 - vector2;
                    angles.Add(Math.Tan(Math.Abs(deviation.Y) / Math.Abs(deviation.X)));
                }
            }

            var averageAngle = angles.Average();
            foreach (var angle in angles)
                Assert.That(angle, Is.EqualTo(averageAngle).Within(10e-2));
        }

        private string RandomString(int length)
        {
            var sb = new StringBuilder();
            var i = 0;
            while (i < length)
            {
                var text = fixture.Create<string>();
                sb.Append(text);
                i += text.Length;
            }
            return sb.ToString().Substring(0, length);
        }
    }
}