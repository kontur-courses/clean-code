using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;
using System.Text;
using Markdown.Data;

namespace Markdown.Tests;

[TestFixture]
class Markdown_Tests
{
    public static IEnumerable<TestCaseData> GenerateHtmlSource()
    {
        yield return new TestCaseData(
            "# main title\n__some bold text__",
            "<h1>main title</h1>\n<strong>some bold text</strong>",
            new []
            {
                new Token(Marks.Header, 0, 12),
                new Token(Marks.Bold, 13, 29)
            }
            ).SetName("Simple text");
        yield return new TestCaseData(
            "# main title\n__some _bold_ text__",
            "<h1>main title</h1>\n<strong>some <em>bold</em> text</strong>",
            new []
            {
                new Token(Marks.Header, 0, 12),
                new Token(Marks.Bold, 13, 31),
                new Token(Marks.Italic, 20, 25)
            }
        ).SetName("Token inside token");
    }
    
    [Test, TestCaseSource(nameof(GenerateHtmlSource))]
    public void GenerateHtml_DifferentText(string actual, string expected, Token[] tokens)
    {
        Md.GenerateHtml(actual, tokens).Should().Be(expected);
    }
    
    public static IEnumerable<TestCaseData> Render_Source()
    {
        yield return new TestCaseData(
            "п# з __ж _к_ ж__ з\nп",
            "п<h1>з <strong>ж <em>к</em> ж</strong> з</h1>\nп"
        ).SetName("Small text for debugging");
        yield return new TestCaseData(
            "# Заголовок __с _разными_ символами__",
            "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>"
        ).SetName("Simple text");
        yield return new TestCaseData(
            "- __bold__\n- _italic_\n- # header",
            "<li><strong>bold</strong></li>\n<li><em>italic</em></li>\n<li><h1>header</h1></li>"
        ).SetName("Simple list");
        yield return new TestCaseData(
            "# Заголовок с# заголовком\n\n _нач_ало се__реди__на ко_нец_ а __также _вложенные_ тэги __сам_ых__ раз_ных__ видов__",
            "<h1>Заголовок с<h1>заголовком</h1></h1>\n\n <em>нач</em>ало се<strong>реди</strong>на ко<em>нец</em> а __также <em>вложенные</em> тэги __сам_ых__ раз_ных__ видов__"
        ).SetName("Normal text");
        yield return new TestCaseData(
            "# Спецификация языка разметки\n\nПосмотрите этот файл в сыром виде. Сравните с тем, что показывает github.\nВсе совпадения случайны ;)\n\n\n\n# Курсив\n\nТекст, _окруженный с двух сторон_ одинарными символами подчерка,\nдолжен помещаться в HTML-тег \\<em> вот так:\n\nТекст, \\<em>окруженный с двух сторон\\</em> одинарными символами подчерка,\nдолжен помещаться в HTML-тег \\<em>.\n\n\n\n# Полужирный\n\n__Выделенный двумя символами текст__ должен становиться полужирным с помощью тега \\<strong>.\n\n\n\n# Экранирование\n\nЛюбой символ можно экранировать, чтобы он не считался частью разметки.\n\\_Вот это\\_, не должно выделиться тегом \\<em>.\n\nСимвол экранирования исчезает из результата, только если экранирует что-то.\nЗдесь сим\\волы экранирования\\ \\должны остаться.\\\n\nСимвол экранирования тоже можно экранировать: \\\\_вот это будет выделено тегом_ \\<em>\n\n\n\n# Взаимодействие тегов\n\nВнутри __двойного выделения _одинарное_ тоже__ работает.\n\nНо не наоборот — внутри _одинарного __двойное__ не_ работает.\n\nПодчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.\n\nОднако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._\n\nВ то же время выделение в ра_зных сл_овах не работает.\n\n__Непарные_ символы в рамках одного абзаца не считаются выделением.\n\nЗа подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением \nи остаются просто символами подчерка.\n\nПодчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки _не считаются_ окончанием выделения \nи остаются просто символами подчерка.\n\nВ случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.\n\nЕсли внутри подчерков пустая строка ____, то они остаются символами подчерка.\n\n\n\n# Заголовки\n\nАбзац, начинающийся с '\\# ', выделяется тегом \\<h1> в заголовок.\nВ тексте заголовка могут присутствовать все прочие символы разметки с указанными правилами.\n\nТаким образом\n\n# Заголовок __с _разными_ символами__\n\nпревратится в:\n\n\\<h1>Заголовок \\<strong>с \\<em>разными\\</em> символами\\</strong>\\</h1>",
            "<h1>Спецификация языка разметки</h1>\n\nПосмотрите этот файл в сыром виде. Сравните с тем, что показывает github.\nВсе совпадения случайны ;)\n\n\n\n<h1>Курсив</h1>\n\nТекст, <em>окруженный с двух сторон</em> одинарными символами подчерка,\nдолжен помещаться в HTML-тег \\<em> вот так:\n\nТекст, \\<em>окруженный с двух сторон\\</em> одинарными символами подчерка,\nдолжен помещаться в HTML-тег \\<em>.\n\n\n\n<h1>Полужирный</h1>\n\n__Выделенный двумя символами текст__ должен становиться полужирным с помощью тега \\<strong>.\n\n\n\n<h1>Экранирование</h1>\n\nЛюбой символ можно экранировать, чтобы он не считался частью разметки.\n\\_Вот это\\_, не должно выделиться тегом \\<em>.\n\nСимвол экранирования исчезает из результата, только если экранирует что-то.\nЗдесь сим\\волы экранирования\\ \\должны остаться.\\\n\nСимвол экранирования тоже можно экранировать: \\\\_вот это будет выделено тегом_ \\<em>\n\n\n\n<h1>Взаимодействие тегов</h1>\n\nВнутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.\n\nНо не наоборот — внутри <em>одинарного __двойное__ не</em> работает.\n\nПодчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.\n\nОднако выделять часть слова они могут: и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>\n\nВ то же время выделение в ра_зных сл_овах не работает.\n\n__Непарные_ символы в рамках одного абзаца не считаются выделением.\n\nЗа подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением \nи остаются просто символами подчерка.\n\nПодчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки <em>не считаются</em> окончанием выделения \nи остаются просто символами подчерка.\n\nВ случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.\n\nЕсли внутри подчерков пустая строка ____, то они остаются символами подчерка.\n\n\n\n<h1>Заголовки</h1>\n\nАбзац, начинающийся с \'\\# \', выделяется тегом \\<h1> в заголовок.\nВ тексте заголовка могут присутствовать все прочие символы разметки с указанными правилами.\n\nТаким образом\n\n<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>\n\nпревратится в:\n\n\\<h1>Заголовок \\<strong>с \\<em>разными\\</em> символами\\</strong>\\</h1>"
        ).SetName("Biggest text");
    }
    
    [Test, TestCaseSource(nameof(Render_Source))]
    public void Render_DifferentText(string actual, string expected)
    {
        Md.Render(actual).Should().Be(expected);
    }

    [Test, Explicit]
    [TestCase(10000, 10)]
    [TestCase(10000, 1000)]
    public void ЕfficiencyTest(long n, long coefficient)
    {
        var input = StackString(n);
        var stopwatch = Stopwatch.StartNew();
        var res = Md.Render(input);
        stopwatch.Stop();
        var time1 = stopwatch.ElapsedMilliseconds * coefficient;
        Console.WriteLine("Normal text time: " + stopwatch.ElapsedMilliseconds);
        input = StackString(n * coefficient);
        stopwatch = Stopwatch.StartNew();
        res = Md.Render(input);
        stopwatch.Stop();
        var time2 = stopwatch.ElapsedMilliseconds;
        Console.WriteLine("Text * koef time: " + stopwatch.ElapsedMilliseconds);
        time2.Should().BeLessThan(time1);
    }

    private string StackString(long times)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < times; i++)
        {
            sb.Append("__a");
        }
        sb.Append("__");
        return sb.ToString();
    }
}