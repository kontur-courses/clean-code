using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownTests
    {
        public Markdown.Markdown Markdown;
        [SetUp]
        public void Setup()
        {
            Markdown = new Markdown.Markdown();
        }

        [TestCaseSource(nameof(RenderData))]
        public void Markdown_Render(string text, string expected)
        {
            var actual = Markdown.Render(text);
            
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
            };
            
            return testData.Select(test
                => new TestCaseData(test.text, test.expected) {TestName = test.testName});
        }
    }
}