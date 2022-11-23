using System;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_Test
    {
        private const string Text = "# заголовок __с жирным текстом__\n" +
                                "Просто текст, в котором _курсивные_ выделения\n" +
                                "__Есть жирный текст__\n" +
                                "__А вот жирный текст _с курсивом_ внутри _и ещё курсив_ в жирном__\n" +
                                "_Вот __это_ не__ сработает\n" +
                                "_И вот так __тоже__ нет_\n" +
                                "Это - _ - просто подчёркивание\n" +
                                "Так_ не работает_\n" +
                                "И _ вот так _ тоже\n" +
                                "В с_лов_е можно выделять, а в цифрах 1_23_ нет\n" +
                                "Ещу можно сделать просто _курсив_\n" +
                                "А можно [ссылку](that's great.com)\n" +
                                "А вот в [_ссылке_](second link.ru) выделения не работают\n" +
                                "Но _[выделенная ссылка](third link.me)_ работает";

            private const string ExpectedResult = "<h1>заголовок <strong>с жирным текстом</strong></h1>\n" +
                                          "Просто текст, в котором <em>курсивные</em> выделения\n" +
                                          "<strong>Есть жирный текст</strong>\n" +
                                          "<strong>А вот жирный текст <em>с курсивом</em> внутри <em>и ещё курсив</em> в жирном</strong>\n" +
                                          "_Вот __это_ не__ сработает\n" +
                                          "_И вот так __тоже__ нет_\n" +
                                          "Это - _ - просто подчёркивание\n" +
                                          "Так_ не работает_\n" +
                                          "И _ вот так _ тоже\n" +
                                          "В с<em>лов</em>е можно выделять, а в цифрах 1_23_ нет\n" +
                                          "Ещу можно сделать просто <em>курсив</em>\n" +
                                          "А можно <a href=\"that's great.com\">ссылку</a>\n" +
                                          "А вот в <a href=\"second link.ru\">_ссылке_</a> выделения не работают\n" +
                                          "Но <em><a href=\"third link.me\">выделенная ссылка</a></em> работает";
        
        [Test]
        public void Render_ShouldReturn_HtmlString()
        {
            Md.Render(Text).Should().Be(ExpectedResult);
        }

        [Test]
        [Timeout(1000)]
        public void Render_ShouldWorkFast_OnBigText()
        {
            var text = string.Concat(Enumerable.Repeat("# Заголовок __с _разными_ символами__", 50000));
            Md.Render(text);
        }
    }
}