using System.Diagnostics;
using FluentAssertions;
using Markdown.Parsers.TagsParsers.Markdown;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class MarkdownTests
{
    [SetUp]
    public void Setup()
    {
        _escapeCharacters = new HashSet<string> { "\\" };
        _parser = new MarkdownParser(_escapeCharacters);
    }

    private HashSet<string> _escapeCharacters;
    private MarkdownParser _parser;

    private static readonly IEnumerable<TestCaseData> RenderStrongTags = new[]
    {
        new TestCaseData("__окруженный с двух сторон__").Returns(@"<strong>окруженный с двух сторон</strong>")
            .SetName("SurrondedByTwoStrongTags"),
        new TestCaseData("в __середине__ текста").Returns(@"в <strong>середине</strong> текста")
            .SetName("WithTwoStrongTagsInTheMiddle"),
        new TestCaseData("__1подчеркивание цифр снуражи текста__")
            .Returns(@"<strong>1подчеркивание цифр снуражи текста</strong>")
            .SetName("WithMappedStrongTagUnderliningNumberOutsideTextAtStart"),
        new TestCaseData("__подчеркивание цифр снуражи текста1__")
            .Returns(@"<strong>подчеркивание цифр снуражи текста1</strong>")
            .SetName("WithMappedStrongTagUnderliningNumberOutsideTextAtEnd")
    };

    private static readonly IEnumerable<TestCaseData> RenderEmTags = new[]
    {
        new TestCaseData("_окруженный с двух сторон_").Returns(@"<em>окруженный с двух сторон</em>")
            .SetName("SurrondedByTwoEmTagsFormStartToEnd"),
        new TestCaseData("в _середине_ текста").Returns(@"в <em>середине</em> текста")
            .SetName("WithTwoEmTagsInTheMiddle"),
        new TestCaseData("_1подчеркивание цифр снуражи текста_").Returns(@"<em>1подчеркивание цифр снуражи текста</em>")
            .SetName("WithMappedEmTagUnderliningNumberOutsideTextAtStart"),
        new TestCaseData("_подчеркивание цифр снуражи текста1_").Returns(@"<em>подчеркивание цифр снуражи текста1</em>")
            .SetName("WithMappedEmTagUnderliningNumberOutsideTextAtEnd")
    };

    private static readonly IEnumerable<TestCaseData> RenderNotMappedTags = new[]
    {
        new TestCaseData("1_подчеркивание цифр внутри текста_").Returns(@"1_подчеркивание цифр внутри текста_")
            .SetName("WithNotMappedEmTagUnderliningNumberInsideTextAtStart"),
        new TestCaseData("_подчеркивание цифр внутри текста_1").Returns(@"_подчеркивание цифр внутри текста_1")
            .SetName("WithNotMappedEmTagUnderliningNumberInsideTextAtEnd"),
        new TestCaseData("_подчеркиван1_ие цифр внутри текста").Returns(@"_подчеркиван1_ие цифр внутри текста")
            .SetName("WithNotMappedEmTagUnderliningNumberInMiddleOfText"),

        new TestCaseData("1__подчеркивание цифр внутри текста__").Returns(@"1__подчеркивание цифр внутри текста__")
            .SetName("WithNotMappedStrongTagUnderliningNumberInsideTextAtStart"),
        new TestCaseData("__подчеркивание цифр внутри текста__1").Returns(@"__подчеркивание цифр внутри текста__1")
            .SetName("WithNotMappedStrongTagUnderliningNumberInsideTextAtEnd"),
        new TestCaseData("__подчеркиван1__ие цифр внутри текста").Returns(@"__подчеркиван1__ие цифр внутри текста")
            .SetName("WithNotMappedStrongTagUnderliningNumberInMiddleOfText"),


        new TestCaseData("____ пустое выделение").Returns(@"____ пустое выделение")
            .SetName("WithNotMappedStrongTagsSurroundEmptyText"),
        new TestCaseData("пустое__выделение").Returns(@"пустое__выделение")
            .SetName("WithNotMappedEmTagsSurroundEmptyText"),

        new TestCaseData("__незакрытый тэг").Returns(@"__незакрытый тэг")
            .SetName("WithNotMappedNotClosedStrongTag"),
        new TestCaseData("незакрытый_ тэг").Returns(@"незакрытый_ тэг")
            .SetName("WithNotMappedNotClosedEmTag"),

        new TestCaseData(@"тэг абзаца в # середине текста")
            .Returns(@"тэг абзаца в # середине текста")
            .SetName("WithNotMappedParagraphTagInTheMiddleOfText"),

        new TestCaseData(@"тэг списка в * середине текста")
            .Returns(@"тэг списка в * середине текста")
            .SetName("WithNotMappedUnorderedTagInTheMiddleOfText")
    };

    private static readonly IEnumerable<TestCaseData> RenderTagsInDifferentPartsOfWords = new[]
    {
        new TestCaseData(@"тэ_ги в раз_ных словах")
            .Returns(@"тэ_ги в раз_ных словах")
            .SetName("WithNotMappedTagsInDifferentWords"),
        new TestCaseData(@"тэги в се_ре_дине одного слова")
            .Returns(@"тэги в се<em>ре</em>дине одного слова")
            .SetName("WithMappedTagsInsideOneWord"),
        new TestCaseData(@"тэги в начале и _сере_дине одного слова")
            .Returns(@"тэги в начале и <em>сере</em>дине одного слова")
            .SetName("WithMappedTagsInTheStartAndMiddleOfOneWord"),
        new TestCaseData(@"тэги в конце и сере_дине_ одного слова")
            .Returns(@"тэги в конце и сере<em>дине</em> одного слова")
            .SetName("WithMappedTagsInTheEndAndMiddleOfOneWord")
    };

    private static readonly IEnumerable<TestCaseData> RenderEscapeCharacters = new[]
    {
        new TestCaseData(@"экранируемый \_тэг_").Returns(@"экранируемый _тэг_")
            .SetName("WithNotMappedEscapedTag"),
        new TestCaseData(@"экранируемая \\_экранизация_").Returns(@"экранируемая \<em>экранизация</em>")
            .SetName("WithDoubleEscapeCharacterNotEscapingTag"),
        new TestCaseData(@"не\экра\низац\ия").Returns(@"не\экра\низац\ия")
            .SetName("WithEscapeCharactersThatDoesNotEscape")
    };

    private static readonly IEnumerable<TestCaseData> RenderNestedTags = new[]
    {
        new TestCaseData(@"Внутри __двойного выделения _одинарное_ тоже__ работает")
            .Returns(@"Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает")
            .SetName("WithMappedTagsWhenEmInsideStrong"),
        new TestCaseData(@"внутри _одинарного __двойное__ не_ работает.")
            .Returns(@"внутри <em>одинарного __двойное__ не</em> работает.")
            .SetName("WithNotMappedTagsWhenStrongInsideEm")
    };

    private static readonly IEnumerable<TestCaseData> RenderHeaders = new[]
    {
        new TestCaseData(@"# Абзац, начинающийся с")
            .Returns(@"<h1>Абзац, начинающийся с</h1>")
            .SetName("WithMappedParagraphTag"),
        new TestCaseData(@"# Заголовок __с _разными_ символами__")
            .Returns(@"<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")
            .SetName("WithMappedTagsInsideParagraph"),
        new TestCaseData("# Заголовок __с _разными_ символами__\n# Заголовок __с _разными_ символами__")
            .Returns(@"<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>"
                     + @"<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")
            .SetName("WithSeveralParagraphs")
    };

    private static readonly IEnumerable<TestCaseData> RenderUnorderedListTags = new[]
    {
        new TestCaseData("* список из одного элемента").Returns(@"<ul><li>список из одного элемента</li></ul>")
            .SetName("WithUnorderedListOfOneElement"),
        new TestCaseData("* список из двух элементов\n* список из двух элементов")
            .Returns(@"<ul><li>список из двух элементов</li><li>список из двух элементов</li></ul>")
            .SetName("WithUnorderedListOfTwoElements"),
        new TestCaseData("* список из _одного_ элемента")
            .Returns(@"<ul><li>список из <em>одного</em> элемента</li></ul>")
            .SetName("WithUnorderedListWithEmTags"),
        new TestCaseData("* список из __одного__ элемента")
            .Returns(@"<ul><li>список из <strong>одного</strong> элемента</li></ul>")
            .SetName("WithUnorderedListWithStrongTags"),
        new TestCaseData("* список из 1одного1 элемента")
            .Returns(@"<ul><li>список из 1одного1 элемента</li></ul>")
            .SetName("WithUnorderedListWithNumbersInMiddleOfText"),
        new TestCaseData("* 1список из одного элемента")
            .Returns(@"<ul><li>1список из одного элемента</li></ul>")
            .SetName("WithUnorderedListWithNumbersAtStart"),
        new TestCaseData("* список из одного элемента1")
            .Returns(@"<ul><li>список из одного элемента1</li></ul>")
            .SetName("WithUnorderedListWithNumbersAtEnd")
    };

    [TestCaseSource(nameof(RenderEmTags))]
    [TestCaseSource(nameof(RenderHeaders))]
    [TestCaseSource(nameof(RenderStrongTags))]
    [TestCaseSource(nameof(RenderNestedTags))]
    [TestCaseSource(nameof(RenderNotMappedTags))]
    [TestCaseSource(nameof(RenderEscapeCharacters))]
    [TestCaseSource(nameof(RenderUnorderedListTags))]
    [TestCaseSource(nameof(RenderTagsInDifferentPartsOfWords))]
    public string Render_ShouldReturnText(string text)
    {
        return Markdown.Markdown.Render(text, _parser);
    }

    private static readonly IEnumerable<TestCaseData> RenderNoException = new[]
    {
        new TestCaseData("__ тэг начинает абзац и после него пробел")
            .SetName("TextStartsWithTagAndWhiteSpaceAfter"),
        new TestCaseData("тэг заканчивает абзац и перед ним пробел __")
            .SetName("TextStartsWithTagAndWhiteSpaceAfter")
    };

    [TestCaseSource(nameof(RenderNoException))]
    public void Render_ShouldNotThrowExceptionWhen(string text)
    {
        Assert.DoesNotThrow(() => Markdown.Markdown.Render(text, _parser));
    }

    private static readonly IEnumerable<TestCaseData> TextsWithDifferentSizes = new[]
    {
        new TestCaseData("_размер10_", string.Concat(Enumerable.Repeat("_размер10_", 10)))
            .SetName("WhenLengthFactorIs10"),
        new TestCaseData("_размер10_", string.Concat(Enumerable.Repeat("_размер10_", 100)))
            .SetName("WhenLengthFactorIs100"),
        new TestCaseData("_размер10_", string.Concat(Enumerable.Repeat("_размер10_", 1000)))
            .SetName("WhenLengthFactorIs1000"),
        new TestCaseData("_размер10_", string.Concat(Enumerable.Repeat("_размер10_", 10000)))
            .SetName("WhenLengthFactorIs10000")
    };

    [TestCase("прогревочный вызов", "для компиляции метода в машинный код", TestName = "FirstCallToSetUpTheMethod")]
    [TestCaseSource(nameof(TextsWithDifferentSizes))]
    public void Render_ShouldBeOfLinearComplexity(string smallerText, string biggerText)
    {
        var renderSmallTextTime = GetTimeToRenderText(smallerText, _parser);
        var renderBigTextTime = GetTimeToRenderText(biggerText, _parser);

        var lengthFactor = biggerText.Length / smallerText.Length;
        var timeFactor = renderBigTextTime / renderSmallTextTime;
        var isRenderComplexityLinear = timeFactor <= lengthFactor;

        isRenderComplexityLinear.Should().BeTrue();
    }

    private static long GetTimeToRenderText(string text, MarkdownParser parser)
    {
        GC.Collect();
        var stopwatch = new Stopwatch();
        stopwatch.Reset();
        stopwatch.Start();
        Markdown.Markdown.Render(text, parser);
        stopwatch.Stop();

        return stopwatch.ElapsedTicks;
    }
}