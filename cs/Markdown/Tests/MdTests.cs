using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;
using Markdown.TagReaders;


namespace Markdown.Tests;

[TestFixture]
public class MdTests
{
    private Md md = null!;

    private static Md CreateMd() => new(
        new Tokenizer(
            new TagReadersProvider(
            [
                new LinkTagReader(),
                new UnderscoresTagsReader(),
                new HeaderTagReader()
            ])));

    [SetUp]
    public void SetUp() => md = CreateMd();

    [TestCase("текст", "текст")]
    [TestCase("", "")]
    [TestCase("_a_", "<em>a</em>")]
    [TestCase("Текст, _окруженный_ подчерками", "Текст, <em>окруженный</em> подчерками")]
    [TestCase("__a__", "<strong>a</strong>")]
    [TestCase("__a b__ c", "<strong>a b</strong> c")]
    public void Render_Emphasis(string input, string expected)
        => Assert.That(md.Render(input), Is.EqualTo(expected));

    [TestCase(@"\_Вот это\_, не", "_Вот это_, не")]
    [TestCase(@"Здесь сим\волы экранирования\ \должны остаться.\", @"Здесь сим\волы экранирования\ \должны остаться.\")]
    [TestCase(@"\\_вот это будет выделено тегом_ \<em>", @"\<em>вот это будет выделено тегом</em> \<em>")]
    public void Render_Escaping(string input, string expected)
        => Assert.That(md.Render(input), Is.EqualTo(expected));

    [TestCase("Внутри __двойного выделения _одинарное_ тоже__ работает.",
        "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.")]
    [TestCase("внутри _одинарного __двойное__ не_ работает",
        "внутри <em>одинарного __двойное__ не</em> работает")]
    [TestCase("текста c цифрами_12_3 не", "текста c цифрами_12_3 не")]
    [TestCase("в _нач_але, и в сер_еди_не, и в кон_це._",
        "в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>")]
    [TestCase("в ра_зных сл_овах не", "в ра_зных сл_овах не")]
    [TestCase("__Непарные_ символы", "__Непарные_ символы")]
    [TestCase("эти_ подчерки_ не", "эти_ подчерки_ не")]
    [TestCase("эти _подчерки _не считаются", "эти _подчерки _не считаются")]
    [TestCase("__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_")]
    [TestCase("_пересечения __одинарных_ и двойных__", "_пересечения __одинарных_ и двойных__")]
    [TestCase("пустая ____ строка", "пустая ____ строка")]
    [TestCase("пустая __ __ строка", "пустая __ __ строка")]
    public void Render_TagInteraction(string input, string expected)
        => Assert.That(md.Render(input), Is.EqualTo(expected));

    [TestCase("# Заголовок", "<h1>Заголовок</h1>")]
    [TestCase("# Заголовок __с _разными_ символами__",
        "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
    [TestCase("#Не заголовок", "#Не заголовок")]
    [TestCase("текст # не заголовок", "текст # не заголовок")]
    [TestCase("[# a](b)", "<a href=\"b\"># a</a>")]
    public void Render_Headers(string input, string expected)
        => Assert.That(md.Render(input), Is.EqualTo(expected));

    [Test]
    public void Render_PreservesParagraphSeparators()
        => Assert.That(md.Render("a _b_\n\n# h\r\n\r\nc\n"),
            Is.EqualTo("a <em>b</em>\n\n<h1>h</h1>\r\n\r\nc\n"));

    [Test]
    public void Render_EmphasisDoesNotCrossParagraphs()
        => Assert.That(md.Render("_a\n\nb_"), Is.EqualTo("_a\n\nb_"));

    [Test]
    public void Render_NullText_Throws()
        => Assert.Throws<ArgumentNullException>(() => md.Render(null!));

    [TestCase("_a ")]
    [TestCase("_a __b ")]
    [TestCase("_a__b_ __c__ ")
    ]
    public void Render_WorksInLinearTime(string unit)
    {
        const int small = 50_000;
        const int large = 500_000;
        md.Render(string.Concat(Enumerable.Repeat(unit, 1000)));

        var smallTime = Measure(unit, small);
        var largeTime = Measure(unit, large);

        Assert.That(largeTime, Is.LessThan(Math.Max(smallTime * 30, 200)),
            $"small={smallTime}ms, large={largeTime}ms");
    }

    private long Measure(string unit, int repeats)
    {
        var input = string.Concat(Enumerable.Repeat(unit, repeats));
        var watch = Stopwatch.StartNew();
        md.Render(input);
        return watch.ElapsedMilliseconds;
    }
}