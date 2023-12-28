using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace MarkdownTests;

public class MdTests
{
    private readonly Md sut = new Md();
    private static Random random = new();

    [TestCase("_окруженный с двух сторон_", "<em>окруженный с двух сторон</em>",  TestName = "работа тега _")]
    [TestCase("__окруженный с двух сторон__", "<strong>окруженный с двух сторон</strong>", TestName = "работа тега __")]
    [TestCase("# Заголовки", "<h1>Заголовки</h1>", TestName ="работа тега #")]
    [TestCase(@"\_Вот это\_", @"_Вот это_", TestName = "экранирование знаков разметки оставляет знаки нетронутыми, а сами символы экранирования исчезают")]
    [TestCase(@"Здесь сим\волы экранирования\ \должны остаться.\", @"Здесь сим\волы экранирования\ \должны остаться.\", TestName = "знак экранирования исчезает из разметки только если экранирует что-либо")]
    [TestCase(@"\\_вот это будет выделено тегом_", @"\<em>вот это будет выделено тегом</em>", TestName = "экранирование знака экранирования")]
    [TestCase(@"Внутри __двойного выделения _одинарное_ тоже__ работает.", @"Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.", TestName = "внутри тега __ работает тег _")] 
    [TestCase(@"внутри _одинарного __двойное__ не_ работает.", @"внутри <em>одинарного __двойное__ не</em> работает.", TestName = "внутри тега _ не работает тег __")]
    [TestCase(@"Подчерки внутри текста c цифрами_12_3 не считаются выделением", @"Подчерки внутри текста c цифрами_12_3 не считаются выделением", TestName = "подчерки внутри текста с цифрами не работают")]
    [TestCase(@"_нач_але, и в сер_еди_не, и в кон_це._", @"<em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>", TestName = "подчерки работают внутри слов")]
    [TestCase(@"выделение в ра_зных сл_овах не работает", @"выделение в ра_зных сл_овах не работает", TestName = "подчерки внутри слов не работают, если находятся в разных словах")]
    [TestCase(@"__Непарные_ символы в рамках одного абзаца не считаются выделением", @"__Непарные_ символы в рамках одного абзаца не считаются выделением", TestName = "непарные парные теги не работают как выделение")]
    [TestCase(@"эти_ подчерки_ не считаются выделением", @"эти_ подчерки_ не считаются выделением", TestName = "после открывающего парного тега должен быть непробельный символ")]
    [TestCase(@"эти _подчерки _не считаются_ окончанием выделения", @"эти <em>подчерки _не считаются</em> окончанием выделения", TestName = "перед закрывающим парным тегом должены быть непробельный символ")]
    [TestCase(@"В случае __пересечения _двойных__ и одинарных_ подчерков", @"В случае __пересечения _двойных__ и одинарных_ подчерков", TestName = "пересечения разных парных тегов не работают оба")]
    [TestCase(@"Если внутри подчерков пустая строка ____, то они остаются символами подчерка.", @"Если внутри подчерков пустая строка ____, то они остаются символами подчерка.", TestName = "парные теги должны содержать в себе непустую строку")]
    [TestCase(@"# Заголовок __с _разными_ символами__", @"<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>", TestName = "работа тега # с разными тегами внутри")]
    public void Render_ReturnsCorrectString_OnDifferentInput(string input, string expected)
    {
        var md = new Md();

        md.Render(input).Should().Be(expected);
    }
    
    [Test]
    public void Render_WorksCloseToLinearTime()
    {
        const int baseTextLength = 100;
        const int increasedTextLength = baseTextLength * 10;
        const int iterations = 100;

        var text = GenerateInput(iterations);
        var bigText = GenerateInput(increasedTextLength);

        var expected = Measure(text, iterations);
        var actual = Measure(bigText, iterations);
        var precision = expected * 0.1;
        
        actual.Should().BeCloseTo(expected, precision);
    }

    private TimeSpan Measure(string text, int count)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();

        var sw = Stopwatch.StartNew();
        for (var i = 0; i < count; i++)
            sut.Render(text);

        return sw.Elapsed / count / text.Length;
    }

    private static string GenerateInput(int length)
    {
        var tags = new[] { "_", "__", "#"};
        var builder = new StringBuilder(length);
        var index = 0;
        while (index < length)
        {
            AddRandomTag(builder, tags, ref index);

            var part = GetRandomString(8);
            builder.Append(part);
            index += part.Length;

            AddRandomTag(builder, tags, ref index);
        }

        return builder.ToString();
    }

    private static void AddRandomTag(StringBuilder builder, string[] tags, ref int index)
    {
        if (random.Next(2) == 0)
        {
            builder.Append(tags[random.Next(tags.Length)]);
            index++;
        }
    }

    private static string GetRandomString(int length)
    {
        const string chars = " \\ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789\n";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}