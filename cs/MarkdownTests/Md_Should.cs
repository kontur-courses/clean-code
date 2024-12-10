using FluentAssertions;
using Markdown;
using NUnit.Framework;
using System.Diagnostics;
using Markdown.Parsers;
using Markdown.Renderers;

namespace MarkdownTests;

[TestFixture]
public class Md_Should
{
    private HtmlRenderer renderer;
    private MarkdownParser parser;
    private Md md;

    [SetUp]
    public void Setup()
    {
        renderer = new HtmlRenderer();
        parser = new MarkdownParser();
        md = new Md(renderer, parser);
    }

    [Test]
    public void Md_ShouldThrowArgumentNullException_WhenInputIsNull()
    {
        var func = () => md.Render(null!);
        func.Should().Throw<ArgumentNullException>();
    }

    [TestCase("", "", TestName = "InputIsEmpty")]
    [TestCase("Это # не заголовок", 
        "Это # не заголовок", 
        TestName = "InvalidHeaderTags")]
    [TestCase(@"Здесь сим\волы экранирования\ \должны остаться.\", 
        @"Здесь сим\волы экранирования\ \должны остаться.\", 
        TestName = "EscapingSymbols")]
    [TestCase("В ра_зных сл_овах", 
        "В ра_зных сл_овах", 
        TestName = "ItalicInDifferentWords")]
    [TestCase("В ра__зных сл__овах", 
        "В ра__зных сл__овах", 
        TestName = "StrongInDifferentWords")]
    [TestCase("Это __непарные _символы в одном абзаце.", 
        "Это __непарные _символы в одном абзаце.", 
        TestName = "UnclosedTagsInMiddle")]
    [TestCase("_e __e", 
        "_e __e", 
        TestName = "UnclosedTagsInStart")]
    [TestCase("e_ e__", 
        "e_ e__", 
        TestName = "UnclosedTagsInEnd")]
    [TestCase("Если пустая _______ строка", 
        "Если пустая _______ строка", 
        TestName = "EmptyTags")]
    [TestCase("_e __s e_ s__", 
        "_e __s e_ s__", 
        TestName = "TagsIntersection")]
    [TestCase("__s \n s__, _e \r\n e_", 
        "__s \n s__, _e \r\n e_", 
        TestName = "TagsIntersectionNewLines")]
    [TestCase("Текст с цифрами_12_3 не должен выделяться", 
        "Текст с цифрами_12_3 не должен выделяться", 
        TestName = "UnderscoreInNumbers")]
    [TestCase("Текст с [незавершённой ссылкой](http://link.com", 
        "Текст с [незавершённой ссылкой](http://link.com", 
        TestName = "UnfinishedUrlInLinkTag")]
    [TestCase("Текст с [незавершённой ссылкой(http://link.com)", 
        "Текст с [незавершённой ссылкой(http://link.com)", 
        TestName = "UnfinishedTextInLinkTag")]
    public void Md_ShouldNotRender_When(string input, string expected)
    {
        var result = md.Render(input);
        result.Should().Be(expected);
    }

    [TestCase("Это _курсив_ и __полужирный__ текст", 
        "Это <em>курсив</em> и <strong>полужирный</strong> текст", 
        TestName = "ItalicAndStrongTags")]
    [TestCase("# Заголовок", 
        "<h1>Заголовок</h1>", 
        TestName = "HeaderTags")]
    [TestCase("# Заголовок __с _разными_ символами__", 
        "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>", 
        TestName = "HeaderWithNestedTags")]
    [TestCase("Это __полужирный _текст_, _с курсивом_ внутри__", 
        "Это <strong>полужирный <em>текст</em>, <em>с курсивом</em> внутри</strong>", 
        TestName = "ItalicInStrong")]
    [TestCase("Это _курсив с __полужирным__ внутри_", 
        "Это <em>курсив с __полужирным__ внутри</em>", 
        TestName = "StrongInItalic")]
    [TestCase(@"Экранированный \_символ\_",
        "Экранированный _символ_", 
        TestName = "EscapeTag")]
    [TestCase("_подчерки _не считаются_", 
        "_подчерки <em>не считаются</em>", 
        TestName = "SpaceBeforeEndOfTag")]
    [TestCase(@"\\_вот это будет выделено тегом_", 
        "<em>вот это будет выделено тегом</em>", 
        TestName = "EscapedYourselfOnStartOfTag")]
    [TestCase(@"_e\\_", 
        "<em>e</em>", 
        TestName = "EscapedYourselfOnEndOfTag")]
    [TestCase("# Заголовок 1\n# Заголовок 2", 
        "<h1>Заголовок 1</h1>\n<h2>Заголовок 2</h2>", 
        TestName = "MultipleHeaders")]
    [TestCase("# h __E _e_ E__ _e_", 
        "<h1>h <strong>E <em>e</em> E</strong> <em>e</em></h1>", 
        TestName = "LotNestedTags")]
    [TestCase("# h __s _E _e_ E_ s__ _e_", 
        "<h1>h <strong>s <em>E <em>e</em> E</em> s</strong> <em>e</em></h1>", 
        TestName = "LotNestedTagsWithDoubleItalic")]
    [TestCase("en_d._, mi__dd__le, _sta_rt", 
        "en<em>d.</em>, mi<strong>dd</strong>le, <em>sta</em>rt", 
        TestName = "BoundedTagsInOneWord")]
    [TestCase("Это текст с [ссылкой](http://link.com)", 
        @"Это текст с <a href=""http://link.com"">ссылкой</a>", 
        TestName = "SimpleLinkTag")]
    [TestCase("Это текст с [двумя](http://link1.com) [ссылками](http://link2.com)", 
        @"Это текст с <a href=""http://link1.com"">двумя</a> <a href=""http://link2.com"">ссылками</a>", 
        TestName = "SeveralLinkTags")]
    [TestCase("_[Ссылка](http://link.com) внутри курсива_", 
        @"<em><a href=""http://link.com"">Ссылка</a> внутри курсива</em>", 
        TestName = "LinkInsideItalic")]
    [TestCase("__[Ссылка](http://link.com) внутри полужирного__", 
        @"<strong><a href=""http://link.com"">Ссылка</a> внутри полужирного</strong>", 
        TestName = "LinkInsideStrong")]
    [TestCase("# [Ссылка](http://link.com)", 
        @"<h1><a href=""http://link.com"">Ссылка</a></h1>", 
        TestName = "LinkInsideHeader")]
    [TestCase("# h __E _e_ [ссылка](http://link.com) E__ _e_", 
        @"<h1>h <strong>E <em>e</em> <a href=""http://link.com"">ссылка</a> E</strong> <em>e</em></h1>", 
        TestName = "NestedTagsWithLink")]
    [TestCase("Это [ссылка с _тегом_](http://link.com)", 
        @"Это <a href=""http://link.com"">ссылка с <em>тегом</em></a>", 
        TestName = "LinkWithTagInside")]
    [TestCase("Пустая ссылка [](http://link.com)", 
        @"Пустая ссылка <a href=""http://link.com""></a>", 
        TestName = "LinkWithEmptyText")]
    [TestCase("Пустая ссылка []()", 
        @"Пустая ссылка <a href=""""></a>", 
        TestName = "LinkWithEmptyUrl")]
    [TestCase(@"[Ссылка с экранированными \] символами\]](http://link.com)", 
        @"<a href=""http://link.com"">Ссылка с экранированными ] символами]</a>", 
        TestName = "LinkWithEscapedSymbol")]
    [TestCase("Это [ссылка с [ссылка с _тегом_](http://link.com)](http://link.com)", 
        @"Это <a href=""http://link.com"">ссылка с <a href=""http://link.com"">ссылка с <em>тегом</em></a></a>", 
        TestName = "LinkInsideLink")]
    public void Md_ShouldRender_When(string input, string expected)
    {
        var result = md.Render(input);
        result.Should().Be(expected);
    }

    [Test]
    public void Md_ShouldRenderLargeInputQuickly()
    {
        var largeInput = string.Concat(Enumerable.Repeat("_Пример_ ", 1000));
        var expectedOutput = string.Concat(Enumerable.Repeat("<em>Пример</em> ", 1000));
        var stopwatch = new Stopwatch();

        stopwatch.Start();
        var result = md.Render(largeInput);
        stopwatch.Stop();

        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000);
        expectedOutput.Should().BeEquivalentTo(result);
    }
}