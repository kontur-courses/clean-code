using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
[TestOf(typeof(Md))]
public class MdTests
{
    private IRenderer _md;

    [SetUp]
    public void SetUp()
    {
        _md = DefaultMdFactory.CreateMd();
    }
    
    [Test]
    [Description("Базовые тесты")]
    [TestCase("", "")]
    [TestCase("Hello world", "Hello world")]
    [TestCase("Hello _world_!", "Hello <em>world</em>!")]
    [TestCase("# _Hello_ __world__!", "<h1><em>Hello</em> <strong>world</strong>!</h1>")]
    public void Render_ReturnsCorrectMarkdown_ForSimpleCases(
        string input,
        string expectedOutput)
    {
        _md.Render(input)
            .Should()
            .Be(expectedOutput);
    }

    [Test]
    [Description("Тесты на вложенность двойного и одинарного выделения")]
    [TestCase("This __text _contains_ nested__ markdown", "This <strong>text <em>contains</em> nested</strong> markdown")]
    [TestCase("This is _an example __of inversed__ nested_ markdown", "This is <em>an example __of inversed__ nested</em> markdown")]
    public void Render_ReturnsCorrectMarkdown_ForCasesWithNesting(
        string input,
        string expectedOutput)
    {
        _md.Render(input)
            .Should()
            .Be(expectedOutput);
    }
    
    [Test]
    [Description("Тесты для разметки внутри текста с цифрами")]
    [TestCase("Text_12_3", "Text_12_3")]
    [TestCase("5__12_3__4", "5__12_3__4")]
    [TestCase("Text __that_12_3__ is in bold", "Text <strong>that_12_3</strong> is in bold")]
    public void Render_ReturnsCorrectMarkdown_ForTextWithNumbers(
        string input,
        string expectedOutput)
    {
        _md.Render(input)
            .Should()
            .Be(expectedOutput);
    }
    
    [Test]
    [Description("Тесты для разметки внутри слов")]
    [TestCase("_begin_ning", "<em>begin</em>ning")]
    [TestCase("mi_ddl_e", "mi<em>ddl</em>e")]
    [TestCase("end_ing_", "end<em>ing</em>")]
    [TestCase("__begin__ning", "<strong>begin</strong>ning")]
    [TestCase("mi__ddl__e", "mi<strong>ddl</strong>e")]
    [TestCase("end__ing__", "end<strong>ing</strong>")]
    public void Render_ReturnsCorrectMarkdown_ForPartsOfWords(
        string input,
        string expectedOutput)
    {
        _md.Render(input)
            .Should()
            .Be(expectedOutput);
    }
    
    [Test]
    [Description("Тесты для подчерков, находящихся внутри разных слов")]
    [TestCase("This sh_ould not cha_nge", "This sh_ould not cha_nge")]
    [TestCase("As w__ell a__s this", "As w__ell a__s this")]
    [TestCase("This sh__o_uld_ wo__rk like this", "This sh__o<em>uld</em> wo__rk like this")]
    public void Render_ReturnsCorrectMarkdown_ForMarkdownInDifferentWords(
        string input,
        string expectedOutput)
    {
        _md.Render(input)
            .Should()
            .Be(expectedOutput);
    }
    
    [Test]
    [Description("Тесты для непарных символов разметки")]
    [TestCase("__Unpaired_ markdown", "__Unpaired_ markdown")]
    [TestCase("Another _unpaired markdown__", "Another _unpaired markdown__")]
    public void Render_ReturnsCorrectMarkdown_ForUnpairedMarkdownSymbols(
        string input,
        string expectedOutput)
    {
        _md.Render(input)
            .Should()
            .Be(expectedOutput);
    }
    
    [Test]
    [Description("Проверяем, что подчерки должны следовать за (стоять перед) непробельным символом")]
    [TestCase("This_ should not_ change", "This_ should not_ change")]
    [TestCase("This _should _be in_ italics", "This <em>should _be in</em> italics")]
    public void Render_ReturnsCorrectMarkdown_ForIncorrectlyPlacedUnderscores(
        string input,
        string expectedOutput)
    {
        _md.Render(input)
            .Should()
            .Be(expectedOutput);
    }
    
    [Test]
    [Description("Тесты на пересечение двойных и одинарных подчерков")]
    [TestCase("Intersecting _markdown __should_ work__ like this", "Intersecting _markdown __should_ work__ like this")]
    [TestCase("Another __example of _intersecting__ markdown_", "Another __example of _intersecting__ markdown_")]
    public void Render_ReturnsCorrectMarkdown_ForIntersectingMarkdown(
        string input,
        string expectedOutput)
    {
        _md.Render(input)
            .Should()
            .Be(expectedOutput);
    }
    
    [Test]
    [Description("Тесты на пустую разметку")]
    [TestCase("This should ____ remain the same", "This should ____ remain the same")]
    [TestCase("This also should __ not change", "This also should __ not change")]
    public void Render_ReturnsCorrectMarkdown_ForEmptyMarkdown(
        string input,
        string expectedOutput)
    {
        _md.Render(input)
            .Should()
            .Be(expectedOutput);
    }
    
    [Test]
    [Description("Тесты на экранирование")]
    [TestCase(@"This should \_not turn\_ into tags", "This should _not turn_ into tags")]
    [TestCase(@"This should \remain the\ same", @"This should \remain the\ same")]
    public void Render_ReturnsCorrectMarkdown_ForEscapeCharacters(
        string input,
        string expectedOutput)
    {
        _md.Render(input)
            .Should()
            .Be(expectedOutput);
    }
}