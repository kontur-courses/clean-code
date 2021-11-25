using System;
using System.Diagnostics;
using System.IO;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    [TestFixture]
    public class MdShould
    {
        private Md md = new Md();

        [Test]
        public void ThrowException_WhenTextIsEmpty()
        {
            Assert.Throws<ArgumentException>(() =>
                md.Render(""));
        }

        [Test]
        public void ThrowException_WhenTextINull()
        {
            Assert.Throws<ArgumentException>(() =>
                md.Render(null));
        }


        [TestCase("# Заголовок1",
            "<h1> Заголовок1</h1>")]
        [TestCase("#Заголовок 1\r\n#Заголовок 2", 
            "<h1>Заголовок 1</h1>\r\n<h1>Заголовок 2</h1>")]
        [TestCase("#Заголовок __с _разными_ символами__",
            "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
        public void RenderHeader(string text, string expected)
        {
            TestRender(text, expected);
        }

        [TestCase("Не Заголовок1",
            "<div>Не Заголовок1</div>")]
        [TestCase("Не Заголовок 1\r\nНе Заголовок 2", 
            "<div>Не Заголовок 1</div>\r\n<div>Не Заголовок 2</div>")]
        [TestCase("#Заголовок1\r\nНе Заголовок 1\r\nНе Заголовок 2",
            "<h1>Заголовок1</h1>\r\n<div>Не Заголовок 1</div>\r\n<div>Не Заголовок 2</div>")]
        public void RenderParagraphs(string text, string expected)
        {
            TestRender(text, expected);
        }

        [TestCase("__Выделенный двумя символами текст__ должен становиться полужирным",
            "<div><strong>Выделенный двумя символами текст</strong> должен становиться полужирным</div>")]
        public void RenderStrong(string text, string expected)
        {
            TestRender(text, expected);
        }

        [TestCase("Текст, _окруженный с двух сторон_ одинарными символами подчерка",
            "<div>Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка</div>")]
        public void RenderItalic(string text, string expected)
        {
            TestRender(text, expected);
        }

        [TestCase("Внутри __двойного выделения _одинарное_ тоже__ работает.",
            "<div>Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.</div>")]

        [TestCase("Но не наоборот — внутри _одинарного __двойное__ не_ работает.",
            "<div>Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает.</div>")]

        [TestCase("Подчерки внутри текста c цифрами_12_3 не считаются выделением",
            "<div>Подчерки внутри текста c цифрами_12_3 не считаются выделением</div>")]

        [TestCase("Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._",
            "<div>Однако выделять часть слова они могут: и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em></div>")]

        [TestCase("В то же время выделение в ра_зных сл_овах не работает",
           "<div>В то же время выделение в ра_зных сл_овах не работает</div>")]
        
        [TestCase("__Непарные_ символы в рамках одного абзаца не считаются выделением",
            "<div>__Непарные_ символы в рамках одного абзаца не считаются выделением</div>")]

        [TestCase("эти_ подчерки_ не считаются выделением",
            "<div>эти_ подчерки_ не считаются выделением</div>")]

        [TestCase("эти _подчерки _не считаются _окончанием выделения",
            "<div>эти _подчерки _не считаются _окончанием выделения</div>")]

        [TestCase("В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением",
            "<div>В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением</div>")]

        [TestCase("В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением",
            "<div>В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением</div>")]

        [TestCase("Если внутри подчерков пустая строка ____, то они остаются символами подчерка",
            "<div>Если внутри подчерков пустая строка ____, то они остаются символами подчерка</div>")]

        public void RenderTagsWithInteraction(string text, string expected)
        {
            TestRender(text, expected);
        }

        [TestCase("\\_Вот это\\_, не должно выделиться тегом",
            "<div>_Вот это_, не должно выделиться тегом</div>")]
        [TestCase("Здесь сим\\волы экранирования\\ \\должны остаться.\\",
            "<div>Здесь сим\\волы экранирования\\ \\должны остаться.\\</div>")]
        [TestCase("\\\\_вот это будет выделено тегом_",
            "<div><em>вот это будет выделено тегом</em></div>")]
        [TestCase("\\\\\\\\_\\\\вот это будет выделено тегом_",
            "<div>\\<em>\\вот это будет выделено тегом</em></div>")]

        public void RenderTagsWithEscaping(string text, string expected)
        {
            TestRender(text, expected);
        }

        [Test]
        public void RenderWithLinearSpeed()
        {
            var sample = "# Заголовок __с _разными_ символами__";
            var bigText = File.ReadAllText("..\\..\\..\\BigMdText.txt");
            GC.Collect();
            GetAverageTimeOfRender(sample);
            var averageSampleTime = GetAverageTimeOfRender(sample);
            var averageBigTextTime = GetAverageTimeOfRender(bigText);
            averageBigTextTime.Should()
                .BeInRange(averageSampleTime * 0.2, averageSampleTime * 1.2);
        }

        private double GetAverageTimeOfRender(string text)
        {
            var watch = new Stopwatch();
            var symbolsCount = text.Length;
            watch.Start();
            md.Render(text);
            watch.Stop();
            return watch.ElapsedTicks / (double)symbolsCount;
        }


        public void TestRender(string text, string expected)
        {
            var res = md.Render(text);
            res.Should().Be(expected);
        }
    }
}
