using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using NUnit.Framework;

namespace Markdown.Testing
{
    [TestFixture]
    public class Markdown_Tests
    {
         private Markdown markdown;

        [SetUp]
        public void CreateMd()
        {
            markdown = new Markdown();
        }

        [TestCase("_paragraph_", ExpectedResult = "<em>paragraph</em>", TestName = "Нижнее подчеркивание на простой строке")]
        [TestCase("_paragraph par_", ExpectedResult = "<em>paragraph par</em>", TestName = "Несколько слов в нижнем подчеркивании")]
        [TestCase("1_1_1", ExpectedResult = "1_1_1", TestName = "Числа не должны быть окружены тегами")]
        [TestCase(@"\_paragraph\_", ExpectedResult = "_paragraph_", TestName = "Экранирование тега")]
        [TestCase("_ paragraph_", ExpectedResult = "_ paragraph_", TestName = "Пробельный символ после открывающего тега")]
        [TestCase("_paragraph _", ExpectedResult = "_paragraph _", TestName = "Пробельный символ перед закрывающим тегом")]
        public string RenderToHtml_WithSingleUnderscore(string mdString) => markdown.Render(mdString);

        [TestCase("__paragraph__", ExpectedResult = "<strong>paragraph</strong>", TestName = "Двойное подчеркивание в простой строке")]
        [TestCase("__paragraph par__", ExpectedResult = "<strong>paragraph par</strong>", TestName = "Несколько слов в двойном подчеркивании")]
        [TestCase("__1__1", ExpectedResult = "__1__1", TestName = "Числа в двойном подчеркивании")]
        [TestCase(@"\__paragraph\__", ExpectedResult = "__paragraph__", TestName = "Экранирование двойного подчеркивания")]
        [TestCase("__ paragraph__", ExpectedResult = "__ paragraph__", TestName = "Пробельный символ после открывающего тега")]
        [TestCase("__paragraph __", ExpectedResult = "__paragraph __", TestName = "Пробельный символ перед закрывающим тегом")]
        public string RenderToHtml_WithDoubleUnderscore(string mdString) => markdown.Render(mdString);

        [TestCase("__paragraph_", ExpectedResult = "__paragraph_", TestName = "Непарные теги не должны меняться")]
        [TestCase("__paragraph _ab_ paragraph__", ExpectedResult = "<strong>paragraph <em>ab</em> paragraph</strong>", TestName = "Тег Em работает в теге strong")]
        [TestCase("__paragraph__ _paragraph_", ExpectedResult = "<strong>paragraph</strong> <em>paragraph</em>", TestName = "Следующие друг за другом теги")]
        [TestCase("_paragraph __ab__ paragraph_", ExpectedResult = "<em>paragraph __ab__ paragraph</em>", TestName = "Strong в теге Eм работать не должен")]
        public string RenderToHtml_WithDifferentUnderscore(string mdString) => markdown.Render(mdString);


        [Test]
        public void AsymptoticComplexity_MustBeLinear()
        {
            var paragraph = "__paragraph__ _paragraph_";
            var sw = new Stopwatch();
            var points = new List<Point>();
            
            for (int i = 0; i < 10; i++)
            {
                sw.Start();
                markdown.Render(paragraph);
                sw.Stop();
                
                points.Add(new Point(paragraph.Length, sw.Elapsed.Milliseconds ));
                paragraph += " " + paragraph;
                markdown = new Markdown();
            }
            
            Assert.IsTrue(true);
        }
    }
}