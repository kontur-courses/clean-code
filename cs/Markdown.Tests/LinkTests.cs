using FluentAssertions;
using Markdown.Core.Lexing;
using Markdown.Core.Parsing;
using Markdown.Core.Rendering;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class LinkTests
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

    [TestCase("[ссылка](https://example.com)", "<p><a href=\"https://example.com\">ссылка</a></p>",
        TestName = "Простая ссылка")]
    [TestCase("Перед [ссылка](url) после", "<p>Перед <a href=\"url\">ссылка</a> после</p>",
        TestName = "Ссылка в середине текста")]
    [TestCase("[незакрытая ссылка(url)", "<p>[незакрытая ссылка(url)</p>",
        TestName = "Нет закрывающей скобки – остаётся текст")]
    [TestCase("[:текст](url)", "<p><a href=\"url\">:текст</a></p>",
        TestName = "Допустимые символы в тексте ссылки")]
    [TestCase("[ссылка](url с пробелом)", "<p><a href=\"url с пробелом\">ссылка</a></p>",
    TestName = "URL допускает пробелы")]
    [TestCase("[ссылка]url)", "<p>[ссылка]url)</p>",
        TestName = "Нет круглых скобок – остаётся текст")]
    [TestCase("\\[ссылка](url)", "<p>[ссылка](url)</p>",
        TestName = "Экранированная квадратная скобка не образует ссылку")]
    public void Render_ShouldHandleLinks(string markdown, string expectedHtml)
    {
        var html = _markdown.Render(markdown);
        html.Should().Be(expectedHtml);
    }
}
