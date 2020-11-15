using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownTests
    {
        private Markdown.Markdown markdown;
        private static Random random;
        [SetUp]
        public void Setup()
        {
            markdown = new Markdown.Markdown();
            random = new Random();
        }

        [TestCaseSource(nameof(RenderData))]
        public void Markdown_Render(string text, string expected)
        {
            var actual = markdown.Render(text);
            
            Assert.That(actual, Is.EqualTo(expected), $"On text:  \"{text}\"");
        }

        private static IEnumerable<TestCaseData> RenderData()
        {
            (string testName, string text, string expected)[] testData = {
                ("No style", "single", "single"),
                ("Angled", "_text_", "<em>text</em>"),
                ("Bold", "__text__", "<strong>text</strong>"),
                ("Header", "# text", "<h1>text</h1>"),
                
                ("Escape symbol escapes style", "\\_text\\_", "_text_"),
                ("Escape symbol does not disappear when does not escape style", "\\text\\", "\\text\\"),
                
                ("Angled inside bold", 
                    "__двойного выделения _одинарное_ тоже__", 
                    "<strong>двойного выделения <em>одинарное</em> тоже</strong>"),
                ("Bold inside angled, angled style only", 
                    "_одинарного __двойное__ не_", 
                    "<em>одинарного __двойное__ не</em>"),
                
                ("Underscored numbers are underscores", "цифрами_12_3", "цифрами_12_3"),
                ("Underscore start middle", "_нач_але", "<em>нач</em>але"),
                ("Underscore middle middle", "сер_еди_не", "сер<em>еди</em>не"),
                ("Underscore middle end", "кон_це._", "кон<em>це.</em>"),
                ("Underscore in middle of different words does not work", "ра_зных сл_овах", "ра_зных сл_овах"),
                
                ("Unclosed style in paragraph double single", "__Непарные_", "__Непарные_"),
                ("Unclosed style in paragraph double", "__Непарные", "__Непарные"),
                ("Unclosed style in paragraph single", "_Непарные", "_Непарные"),
                
                ("Not a style start when whitespace after style symbol", 
                    "эти_ подчерки_ не считаются выделением", 
                    "эти_ подчерки_ не считаются выделением"),
                ("Not a style end when whitespace before style symbol", 
                    "эти _подчерки _не считаются_ окончанием выделения", 
                    "эти _подчерки <em>не считаются</em> окончанием выделения"),
                
                ("No style when styles intersect", "__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_"),
                
                ("Empty text with style does not change", "____", "____"),
                
                ("Paragraph", "# Заголовок __с _разными_ символами__", "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>"),
                
                ("Two underscores", "_с_ _символами_", "<em>с</em> <em>символами</em>"),
                ("Three underscores", "_a_ _b_  _c_", "<em>a</em> <em>b</em>  <em>c</em>"),
                
                ("Picture", "![описание](https://picture1)", "<img src=\"https://picture1\" alt=\"описание\">"),
                ("Link", "[имя](https://link)", "<a href=\"https://link\">имя</a>"),
            };
            
            return testData.Select(test
                => new TestCaseData(test.text, test.expected) {TestName = test.testName});
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
                Console.WriteLine("Elapsed={0}",stopwatch.ElapsedMilliseconds);
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

        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}