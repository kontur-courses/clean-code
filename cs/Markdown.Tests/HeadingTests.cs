using FluentAssertions;
using Markdown.Core.Lexing;
using Markdown.Core.Parsing;
using Markdown.Core.Rendering;
using NUnit.Framework;
namespace Markdown.Tests;

/// <summary>
/// Тесты заголовков
/// </summary>
[TestFixture]
public class HeadingTests
{
    private Md _markdown;

    [SetUp]
    public void Setup()
    {
        var lexer = new Lexer();
        var parser = new Parser();
        var renderer = new Renderer();
        _markdown = new Md(lexer, parser, renderer);
    }

    [TestCase("# Заголовок", "<h1>Заголовок</h1>", 
        TestName = "Простой заголовок")]
    [TestCase("# Заголовок __с _разными_ символами__", "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>",
    TestName = "Заголовок с вложенными тегами")]
    [TestCase("Текст # не заголовок", "<p>Текст # не заголовок</p>",
        TestName = "Решетка в середине строки — не заголовок")]
    [TestCase("#Заголовок без пробела", "<p>#Заголовок без пробела</p>",
        TestName = "Без пробела после решетки — не заголовок")]
    [TestCase(" ## C пробелом в начале", "<p> ## C пробелом в начале</p>",
        TestName = "Решетка не в первом символе — не заголовок")]
    public void Test(string input, string expectedHtml)
    {
        var html = _markdown.Render(input);
        html.Should().Be(expectedHtml);
    }
}
