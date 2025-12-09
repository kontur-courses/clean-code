using FluentAssertions;

namespace Tests.InlineConvertingTests;

public class MarkupConverterEmphasisTests : MarkupConverterTestBase
{
    [Test]
    public void Convert_WhenSingleUnderscoreEmphasis_ShouldCreateEmTag()
    {
        var markdown = "Текст, _окруженный с двух сторон_ одинарными символами подчерка";
        var expectedHtml = "<p>Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenDoubleUnderscoreEmphasis_ShouldCreateStrongTag()
    {
        var markdown = "__Выделенный двумя символами текст__ должен становиться полужирным";
        var expectedHtml = "<p><strong>Выделенный двумя символами текст</strong> должен становиться полужирным</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenEscapedUnderscores_ShouldNotCreateEmphasis()
    {
        var markdown = @"\_Вот это\_, не должно выделиться тегом";
        var expectedHtml = "<p>_Вот это_, не должно выделиться тегом</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenBackSlashNotEscapingAnything_ShouldRemain()
    {
        var markdown = @"Здесь сим\волы экранирования\ \должны остаться.\";
        var expectedHtml = "<p>Здесь сим\\волы экранирования\\ \\должны остаться.\\</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenEscapedBackslashBeforeUnderscore_ShouldCreateEmphasis()
    {
        var markdown = @"\\_вот это будет выделено тегом_";
        var expectedHtml = "<p>\\<em>вот это будет выделено тегом</em></p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenBoldContainsItalic_ShouldWorkCorrectly()
    {
        var markdown = "Внутри __двойного выделения _одинарное_ тоже__ работает.";
        var expectedHtml = "<p>Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenItalicContainsBold_ShouldNotWork()
    {
        var markdown = "Но не наоборот — внутри _одинарного __двойное__ не_ работает.";
        var expectedHtml = "<p>Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает.</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenUnderscoresInTextWithNumbers_ShouldNotCreateEmphasis()
    {
        var markdown = "Подчерки внутри текста c цифрами_12_3 не считаются выделением";
        var expectedHtml = "<p>Подчерки внутри текста c цифрами_12_3 не считаются выделением</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenEmphasisAtBeginningOfWord_ShouldWork()
    {
        var markdown = "и в _нач_але слова";
        var expectedHtml = "<p>и в <em>нач</em>але слова</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenEmphasisInMiddleOfWord_ShouldWork()
    {
        var markdown = "и в сер_еди_не слова";
        var expectedHtml = "<p>и в сер<em>еди</em>не слова</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenEmphasisAtEndOfWord_ShouldWork()
    {
        var markdown = "и в кон_це._ слова";
        var expectedHtml = "<p>и в кон<em>це.</em> слова</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenEmphasisAcrossDifferentWords_ShouldNotWork()
    {
        var markdown = "В то же время выделение в ра_зных сл_овах не работает.";
        var expectedHtml = "<p>В то же время выделение в ра_зных сл_овах не работает.</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenUnpairedUnderscoresInParagraph_ShouldNotCreateEmphasis()
    {
        var markdown = "__Непарные_ символы в рамках одного абзаца не считаются выделением.";
        var expectedHtml = "<p>__Непарные_ символы в рамках одного абзаца не считаются выделением.</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenUnderscoreFollowedBySpaceAfterOpening_ShouldNotStartEmphasis()
    {
        var markdown = "Иначе эти_ подчерки_ не считаются выделением";
        var expectedHtml = "<p>Иначе эти_ подчерки_ не считаются выделением</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenUnderscoreFollowedBySpaceBeforeOpening_ShouldStartEmphasis()
    {
        var markdown = "Иначе эти _подчерки_ не считаются выделением";
        var expectedHtml = "<p>Иначе эти <em>подчерки</em> не считаются выделением</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenUnderscorePrecededBySpaceBeforeClosing_ShouldNotEndEmphasis()
    {
        var markdown = "Иначе эти _подчерки _не считаются окончанием выделения";
        var expectedHtml = "<p>Иначе эти _подчерки _не считаются окончанием выделения</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenUnderscoreNotPrecededBySpaceBeforeClosing_ShouldEndEmphasis()
    {
        var markdown = "Иначе эти _подчерки_ не считаются окончанием выделения";
        var expectedHtml = "<p>Иначе эти <em>подчерки</em> не считаются окончанием выделения</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenIntersectingDoubleAndSingleUnderscores_ShouldNotCreateEmphasis()
    {
        var markdown = "В случае __пересечения _двойных__ и одинарных_ подчерков";
        var expectedHtml = "<p>В случае __пересечения _двойных__ и одинарных_ подчерков</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenEmptyEmphasisBetweenUnderscores_ShouldNotCreateEmphasis()
    {
        var markdown = "Если внутри подчерков пустая строка ____, то они остаются символами подчерка.";
        var expectedHtml = "<p>Если внутри подчерков пустая строка ____, то они остаются символами подчерка.</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenSingleUnderscoresWithSpacesBetween_ShouldNotCreateEmphasis()
    {
        var markdown = "Если _ здесь есть пробелы _, то теги не создаются";
        var expectedHtml = "<p>Если _ здесь есть пробелы _, то теги не создаются</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenDoubleUnderscoresWithSpacesBetween_ShouldNotCreateEmphasis()
    {
        var markdown = "Если __ здесь есть пробелы __, то теги не создаются";
        var expectedHtml = "<p>Если __ здесь есть пробелы __, то теги не создаются</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenMixedEmphasisWithEscaping_ShouldHandleCorrectly()
    {
        var markdown = "Текст с _курсивом_, __жирным__ и \\_экранированным\\_";
        var expectedHtml = "<p>Текст с <em>курсивом</em>, <strong>жирным</strong> и _экранированным_</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenMultipleParagraphsWithEmphasis_ShouldHandleCorrectly()
    {
        var markdown = $"Первый абзац с _курсивом_{Environment.NewLine}{Environment.NewLine}" +
                       $"Второй абзац с __жирным__";
        var expectedHtml = $"<p>Первый абзац с <em>курсивом</em></p>{Environment.NewLine}" +
                           $"<p>Второй абзац с <strong>жирным</strong></p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenComplexNestedEmphasis_ShouldHandleCorrectly()
    {
        var markdown = "Вот __пример _вложенного_ выделения__ и обычного _курсива_";
        var expectedHtml =
            "<p>Вот <strong>пример <em>вложенного</em> выделения</strong> и обычного <em>курсива</em></p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenOnlyUnderscoresInText_ShouldNotCreateEmphasis()
    {
        var markdown = "Текст с только подчерками: ___ и ___";
        var expectedHtml = "<p>Текст с только подчерками: ___ и ___</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenExampleFromSpecification_ShouldMatchExpected()
    {
        var markdown =
            "За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением";
        var expectedHtml =
            "<p>За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением</p>";

        var actualHtml = Converter.Convert(markdown);

        actualHtml.Should().Be(expectedHtml);
    }
}