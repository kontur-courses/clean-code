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

    private static readonly IEnumerable<TestCaseData> RenderSimpleMappedTags = new[]
    {
        new TestCaseData("_окруженный с двух сторон_").Returns(@"<em>окруженный с двух сторон</em>")
            .SetName("SurrondedByTwoEmTagsFormStartToEnd"),
        new TestCaseData("в _середине_ текста").Returns(@"в <em>середине</em> текста")
            .SetName("WithTwoEmTagsInTheMiddle"),
        new TestCaseData("__окруженный с двух сторон__").Returns(@"<strong>окруженный с двух сторон</strong>")
            .SetName("SurrondedByTwoStrongTags"),
        new TestCaseData("в __середине__ текста").Returns(@"в <strong>середине</strong> текста")
            .SetName("WithTwoStrongTagsInTheMiddle"),
        new TestCaseData("_1 подчеркивание цифр снуражи текста_").Returns(@"<em>1 подчеркивание цифр снуражи текста</em>")
            .SetName("WithMappedEmTagUnderliningNumberOutsideText"),
        new TestCaseData("__подчеркивание цифр снуражи текста 1__").Returns(@"<strong>подчеркивание цифр снуражи текста 1</strong>")
            .SetName("WithMappedStrongTagUnderliningNumberOutsideText"),
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
            .SetName("WithNotMappedParagraphTagInTheMiddleOfText")
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

    [TestCaseSource(nameof(RenderHeaders))]
    [TestCaseSource(nameof(RenderNestedTags))]
    [TestCaseSource(nameof(RenderNotMappedTags))]
    [TestCaseSource(nameof(RenderSimpleMappedTags))]
    [TestCaseSource(nameof(RenderEscapeCharacters))]
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