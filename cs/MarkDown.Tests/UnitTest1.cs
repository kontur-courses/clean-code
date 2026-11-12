using MarkDown;
using NUnit.Framework;
using FluentAssertions;

namespace MarkDown.Tests;

public class Tests
{
    [TestCase(@"\_text\_", "<p>_text_</p>", TestName = "Экранирование спецсимвола")]
    [TestCase(@"\\_text_", @"<p>\<em>text</em></p>", TestName = @"Экранирование экранирования")]
    [TestCase(@"\text", @"<p>\text</p>", TestName = "Экранирование перед обычной буквой, не должно экранироваться")]
    [TestCase(@"\#text", @"<p>#text</p>", TestName = "Экранирование заголовка")]
    [TestCase(@"text\", @"<p>text\</p>", TestName = "Экранирование в конце")]
    [TestCase(@"\_a_", "<p>_a_</p>", TestName = "Открывающий экранирован")]
    [TestCase(@"_a\_", "<p>_a_</p>", TestName = "Закрывающий экранирован")]
    [TestCase(@"_a\__", @"<p><em>a_</em></p>", TestName = "Закрывающий экранирован, но есть третий")]
    [TestCase(@"\\", @"<p>\</p>", TestName = "Только экранированный бэкслеш")]
    public void Md_Shield(string markDown, string html)
    {
        Md.Render(markDown).Should().Be(html);
    }
    
    [TestCase("_text_", "<p><em>text</em></p>", TestName = "Простой курсив")]
    [TestCase("_ text_", "<p>_ text_</p>", TestName = "Открывающий перед пробелом — текст")]
    [TestCase("_text _", "<p>_text _</p>", TestName = "Закрывающий после пробела — текст")]
    [TestCase("_a b_", "<p><em>a b</em></p>", TestName = "Пробел внутри — допустим")]
    [TestCase("_a_ _b_", "<p><em>a</em> <em>b</em></p>", TestName = "Два независимых курсива")]
    [TestCase("a_b_c", "<p>a<em>b</em>c</p>", TestName = "Часть слова — валидно")]
    [TestCase("_нач_але", "<p><em>нач</em>але</p>", TestName = "Выделение в начале слова")]
    [TestCase("сер_еди_не", "<p>сер<em>еди</em>не</p>", TestName = "Выделение в середине слова")]
    [TestCase("кон_це_", "<p>кон<em>це</em></p>", TestName = "Выделение в конце слова")]
    [TestCase("ра_зных сл_овах", "<p>ра_зных сл_овах</p>", TestName = "Между словами — не работает")]
    [TestCase("_text", "<p>_text</p>", TestName = "Непарный открывающий")]
    [TestCase("text_", "<p>text_</p>", TestName = "Непарный закрывающий")]
    [TestCase("_", "<p>_</p>", TestName = "Один _")]
    [TestCase("____", "<p>____</p>", TestName = "Четыре _ — пустое выделение")]
    [TestCase("___", "<p>___</p>", TestName = "Три _ — не курсив, не жирный")]
    public void Md_Italic(string markDown, string html)
    {
        Md.Render(markDown).Should().Be(html);
    }


    [TestCase("__text__", "<p><strong>text</strong></p>", TestName = "Простой полужирный")]
    [TestCase("__ text__", "<p>__ text__</p>", TestName = "Открывающий перед пробелом")]
    [TestCase("__text __", "<p>__text __</p>", TestName = "Закрывающий после пробела")]
    [TestCase("__a b__", "<p><strong>a b</strong></p>", TestName = "Пробел внутри — допустим")]
    [TestCase("__a__ __b__", "<p><strong>a</strong> <strong>b</strong></p>", TestName = "Два независимых")]
    [TestCase("__нач__але", "<p><strong>нач</strong>але</p>", TestName = "Часть слова")]
    [TestCase("__a", "<p>__a</p>", TestName = "Непарный открывающий")]
    [TestCase("a__", "<p>a__</p>", TestName = "Непарный закрывающий")]
    [TestCase("__", "<p>__</p>", TestName = "Два непарных _")]
    [TestCase("__a __b__ c__", "<p><strong>a __b__ c</strong></p>", TestName = "Пересечение жирного, внешний работает работает")]
    public void Md_Bold(string markDown, string html)
    {
        Md.Render(markDown).Should().Be(html);
    }


    [TestCase("__a _b_ c__", "<p><strong>a <em>b</em> c</strong></p>", TestName = "курсив внутри, жирный снаружи — работает")]
    [TestCase("_a __b__ c_", "<p><em>a __b__ c</em></p>", TestName = ";жирный внутри, курсив снаружи — не работает")]
    [TestCase("__a_b__c_", "<p>__a_b__c_</p>", TestName = "Пересечение — оба текстом")]
    [TestCase("___a___", "<p>___a___</p>", TestName = "Три подчеркивания ничего не применяется")]
    public void Md_Interaction(string markDown, string html)
    {
        Md.Render(markDown).Should().Be(html);
    }

    [TestCase("_12_3", "<p>_12_3</p>", TestName = "Цифры между курсивом - не выдеяются")]
    [TestCase("_a1b_", "<p>_a1b_</p>", TestName = "Цифра внутри курисва — не выделяется")]
    [TestCase("__a1b__", "<p>__a1b__</p>", TestName = "Цифра внутри жирного — не выделяется")]
    [TestCase("__1__", "<p>__1__</p>", TestName = "Одна цифра внутри жирного")]
    public void Md_Digits(string markDown, string html)
    {
        Md.Render(markDown).Should().Be(html);
    }

    [TestCase("# text", "<h1>text</h1>", TestName = "Простой заголовок")]
    [TestCase("# __text__", "<h1><strong>text</strong></h1>", TestName = "Жирный в заголовке")]
    [TestCase("# _text_", "<h1><em>text</em></h1>", TestName = "Курсив в заголовке")]
    [TestCase("# Заголовок __с _разными_ символами__", "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>", TestName = "Из задания — комбинированный заголовок")]
    [TestCase("#", "<p>#</p>", TestName = "Один # — не заголовок")]
    [TestCase("#text", "<p>#text</p>", TestName = "# без пробела — не заголовок")]
    [TestCase("## text", "<p>## text</p>", TestName = "Только h1 уровень")]
    public void Md_Header(string markDown, string html)
    {
        Md.Render(markDown).Should().Be(html);
    }


    [TestCase("", "", TestName = "Пустая строка")]
    [TestCase("   \n   ", "", TestName = "Только пробелы")]
    [TestCase("a\nb", "<p>a</p><p>b</p>", TestName = "Два абзаца")]
    [TestCase("# a\n# b", "<h1>a</h1><h1>b</h1>", TestName = "Два заголовка")]
    public void Md_another_paragraph(string markDown, string html)
    {
        Md.Render(markDown).Should().Be(html);
    }
}