using FluentAssertions;
using Markdown.Core.Lexing;
using Markdown.Core.Parsing;
using Markdown.Core.Rendering;
using NUnit.Framework;

namespace Markdown.Tests;

/// <summary>
/// Тесты жирного шрифта
/// </summary>
[TestFixture]
public class StrongTests
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

    [TestCase("__полужирный__", "<p><strong>полужирный</strong></p>",
        TestName = "Одно слово полужирным шрифтом ")]
    [TestCase("__Выделенный двумя символами текст__ должен становиться полужирным",
        "<p><strong>Выделенный двумя символами текст</strong> должен становиться полужирным</p>",
        TestName = "Полужирный в предложении")]
    [TestCase("сло__во__ внутри слова", "<p>сло<strong>во</strong> внутри слова</p>",
        TestName = "Двойное выделение внутри слова")]
    [TestCase("Эти __ подчерки__ не работают", "<p>Эти __ подчерки__ не работают</p>",
        TestName = "Не начинается, если после __ пробел")]
    [TestCase("Эти __подчерки __ работают", "<p>Эти <strong>подчерки </strong> работают</p>",
        TestName = "Двойное выделение допускает пробел внутри")]
    public void Test(string inputText, string expectedHtml)
    {
        var html = _markdown.Render(inputText);
        html.Should().Be(expectedHtml);
    }
}

