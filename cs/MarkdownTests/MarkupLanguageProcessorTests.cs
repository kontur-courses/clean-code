using FluentAssertions;
using Markdown;
using Markdown.Inlines;

namespace MarkdownTests;

[TestFixture]
public class MarkupLanguageProcessorTests
{
    private Md _md = null!;

    [SetUp]
    public void SetUp()
    {
        _md = new Md(new BlockSegmenter(), new InlineParser(), new HtmlRenderer());
    }

    [Test]
    public void Render_EmSimpleText_AddsEmTag()
    {
        var input = "Текст, _окруженный с двух сторон_ одинарными символами подчерка";

        var html = _md.Render(input);

        html.Should().Be("<p>Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка</p>");
    }

    // Проверяем пример из спецификацииЖ текст с <em>какой-то текст</em> должен обернуться в <p>, но сами теги остаются как есть
    [Test]
    public void Render_TextWithHtmlTags_KeepsHtmlTags()
    {
        var input = "Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка, должен помещаться в HTML-тег <em>.";

        var html = _md.Render(input);

        html.Should().Be("<p>Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка, должен помещаться в HTML-тег <em>.</p>");
    }

    [Test]
    public void Render_StrongSimpleText_AddsStrongTag()
    {
        var input = "__Выделенный двумя символами текст__ должен становиться полужирным с помощью тега <strong>.";

        var html = _md.Render(input);

        html.Should().Be("<p><strong>Выделенный двумя символами текст</strong> должен становиться полужирным с помощью тега <strong>.</p>");
    }

    [Test]
    public void Render_EscapedUnderscore_ShowsPlainUnderscore()
    {
        var input = @"\_Вот это\_, не должно выделиться тегом <em>.";

        var html = _md.Render(input);

        html.Should().Be("<p>_Вот это_, не должно выделиться тегом <em>.</p>");
    }

    [Test]
    public void Render_BackslashBeforeNormalChar_StaysInText()
    {
        var input = @"Здесь сим\волы экранирования\ \должны остаться.\";

        var html = _md.Render(input);

        html.Should().Be("<p>Здесь сим\\волы экранирования\\ \\должны остаться.\\</p>");
    }

    // Последовательность \\_ дает один символ '\' и выделение _какой-то текст_
    [Test]
    public void Render_EscapedBackslashBeforeEmTag_ShowsBackslashAndEmTag()
    {
        var input = @"Символ экранирования тоже можно экранировать: \\_вот это будет выделено тегом_ <em>";

        var html = _md.Render(input);

        html.Should().Be("<p>Символ экранирования тоже можно экранировать: \\<em>вот это будет выделено тегом</em> <em></p>");
    }

    [Test]
    public void Render_EscapedStrongMarkers_ShowsPlainText()
    {
        var input = @"\__стронг__";

        var html = _md.Render(input);

        html.Should().Be("<p>__стронг__</p>");
    }

    [Test]
    public void Render_EscapedHash_DoesNotMakeHeading()
    {
        var input = @"\# Не заголовок";

        var html = _md.Render(input);

        html.Should().Be("<p># Не заголовок</p>");
    }

    [Test]
    public void Render_BackslashAtEnd_StaysInText()
    {
        var input = @"abc\";

        var html = _md.Render(input);

        html.Should().Be("<p>abc\\</p>");
    }

    // В заголовке экранирования и inline-выделение работают по таким же правилам, что и в обычном тексте
    [Test]
    public void Render_H1WithEscapes_ShowsCorrectHeading()
    {
        var input = @"# Тест \_эм\_ и \\__стронг__";

        var html = _md.Render(input).ReplaceLineEndings(string.Empty);

        html.Should().Be("<h1>Тест _эм_ и \\<strong>стронг</strong></h1>");
    }

    [Test]
    public void Render_EscapedBackslashBeforeStrong_AddsStrongTag()
    {
        var input = @"\\__a__";

        var html = _md.Render(input);

        html.Should().Be("<p>\\<strong>a</strong></p>");
    }

    [Test]
    public void Render_EmInsideStrong_AddsEmAndStrongTags()
    {
        var input = "Внутри __двойного выделения _одинарное_ тоже__ работает.";

        var html = _md.Render(input);

        html.Should().Be("<p>Внутри <strong>двойного выделения </strong><em>одинарное</em><strong> тоже</strong> работает.</p>");
    }

    // Непарные подчеркивания в абзаце не дают ни <em>, ни <strong>
    [Test]
    public void Render_StrongInsideEm_DoesNotAddStrongTag()
    {
        var input = "Но не наоборот — внутри _одинарного __двойное__ не_ работает.";

        var html = _md.Render(input);

        html.Should().Be("<p>Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает.</p>");
    }

    [Test]
    public void Render_UnpairedUnderscoresInParagraph_DoesNotAddAnyTags()
    {
        var input = "__Непарные_ символы в рамках одного абзаца не считаются выделением.";

        var html = _md.Render(input);

        html.Should().Be("<p>__Непарные_ символы в рамках одного абзаца не считаются выделением.</p>");
    }

    // При пустоте между подчеркиваниями ____ все подчеркивания остаются в тексте
    [Test]
    public void Render_CrossingStrongAndEm_DoesNotAddTags()
    {
        var input = "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.";

        var html = _md.Render(input);

        html.Should().Be("<p>В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.</p>");
    }

    [Test]
    public void Render_UnderscoreBetweenDigits_DoesNotStartEmTag()
    {
        var input = "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.";

        var html = _md.Render(input);

        html.Should().Be("<p>Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.</p>");
    }

    [Test]
    public void Render_StrongMarkersBetweenDigits_DoesNotAddStrongTag()
    {
        var input = "цифры__12__3";

        var html = _md.Render(input);

        html.Should().Be("<p>цифры__12__3</p>");
    }

    [Test]
    public void Render_OpeningUnderscoreBeforeSpace_DoesNotStartEmTag()
    {
        var input = "За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.";

        var html = _md.Render(input);

        html.Should().Be("<p>За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.</p>");
    }

    [Test]
    public void Render_ClosingUnderscoreAfterSpace_DoesNotEndEmTag()
    {
        var input = "эти _подчерки _ не считаются окончанием выделения";

        var html = _md.Render(input);

        html.Should().Be("<p>эти _подчерки _ не считаются окончанием выделения</p>");
    }

    [Test]
    public void Render_EmptyBetweenUnderscores_KeepsAllUnderscores()
    {
        var input = "Если внутри подчерков пустая строка ____, то они остаются символами подчерка.";

        var html = _md.Render(input);

        html.Should().Be("<p>Если внутри подчерков пустая строка ____, то они остаются символами подчерка.</p>");
    }

    [Test]
    public void Render_ClosingUnderscoreAfterWordSpace_DoesNotEndEmTag()
    {
        var input = "a _b _ c";

        var html = _md.Render(input);

        html.Should().Be("<p>a _b _ c</p>");
    }

    [Test]
    public void Render_OnlyDoubleUnderscore_KeepsDoubleUnderscore()
    {
        var input = "__";

        var html = _md.Render(input);

        html.Should().Be("<p>__</p>");
    }

    [Test]
    public void Render_EmInsideWordAtStart_AddsEmTag()
    {
        var input = "в _нач_але";

        var html = _md.Render(input);

        html.Should().Be("<p>в <em>нач</em>але</p>");
    }

    [Test]
    public void Render_EmInsideWordInMiddle_AddsEmTag()
    {
        var input = "сер_еди_не";

        var html = _md.Render(input);

        html.Should().Be("<p>сер<em>еди</em>не</p>");
    }

    [Test]
    public void Render_EmInsideWordAtEnd_AddsEmTag()
    {
        var input = "в кон_це._";

        var html = _md.Render(input);

        html.Should().Be("<p>в кон<em>це.</em></p>");
    }

    [Test]
    public void Render_EmAcrossTwoWords_DoesNotAddEmTag()
    {
        var input = "В то же время выделение в ра_зных сл_овах не работает.";

        var html = _md.Render(input);

        html.Should().Be("<p>В то же время выделение в ра_зных сл_овах не работает.</p>");
    }

    [TestCase("и в __нач__але", "и в <strong>нач</strong>але",
        TestName = "Render_StrongInsideWordAtStart_AddsStrongTag")]
    [TestCase("сер__еди__не", "сер<strong>еди</strong>не",
        TestName = "Render_StrongInsideWordInMiddle_AddsStrongTag")]
    [TestCase("и в кон__це__.", "и в кон<strong>це</strong>.",
        TestName = "Render_StrongInsideWordAtEnd_AddsStrongTag")]
    public void Render_StrongInsideWord_AddsStrongTag(string input, string expectedInnerHtml)
    {
        var html = _md.Render(input);

        html.Should().Be($"<p>{expectedInnerHtml}</p>");
    }

    [TestCase("_a_,", "<em>a</em>,",
        TestName = "Render_EmWithCommaAfter_ShowsEmBeforeComma")]
    [TestCase("._b_", ".<em>b</em>",
        TestName = "Render_EmWithDotBefore_ShowsEmAfterDot")]
    [TestCase("_abc_ в начале", "<em>abc</em> в начале",
        TestName = "Render_EmAtLineStart_ShowsEmAtStart")]
    public void Render_EmWithPunctuation_ShowsCorrectHtml(string input, string expectedInnerHtml)
    {
        var html = _md.Render(input);

        html.Should().Be($"<p>{expectedInnerHtml}</p>");
    }

    [Test]
    public void Render_StrongAtLineEnd_AddsStrongTag()
    {
        var input = "в конце __abc__";

        var html = _md.Render(input);

        html.Should().Be("<p>в конце <strong>abc</strong></p>");
    }

    [Test]
    public void Render_PunctuationBeforeOpeningUnderscore_AllowsEmTag()
    {
        var input = "!_a_";

        var html = _md.Render(input);

        html.Should().Be("<p>!<em>a</em></p>");
    }

    [Test]
    public void Render_H1Simple_AddsH1Tag()
    {
        var input = "# Заголовок";

        var html = _md.Render(input);

        html.Should().Be("<h1>Заголовок</h1>");
    }

    [Test]
    public void Render_TwoParagraphsSimple_ShowsTwoParagraphTags()
    {
        var input = "первый абзац\n\nвторой абзац";

        var html = _md.Render(input).ReplaceLineEndings(string.Empty);

        html.Should().Be("<p>первый абзац</p><p>второй абзац</p>");
    }

    [Test]
    public void Render_H1ThenParagraphWithTags_ShowsCorrectHtml()
    {
        var input =
            "# Заголовок __с _разными_ символами__\n\n" +
            "Текст про _эм_ и __стронг__.";

        var html = _md.Render(input).ReplaceLineEndings(string.Empty);

        html.Should().Be("<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1><p>Текст про <em>эм</em> и <strong>стронг</strong>.</p>");
    }

    // Подчеркивания не могут открыть выделение в одном абзаце и закрыть в другом
    [Test]
    public void Render_EmAcrossParagraphs_DoesNotJoinParagraphs()
    {
        var input = "Начало _абзаца\n\nпродолжение_ абзаца";

        var html = _md.Render(input).ReplaceLineEndings(string.Empty);

        html.Should().Be("<p>Начало _абзаца</p><p>продолжение_ абзаца</p>");
    }

    [Test]
    public void Render_SpacesBetweenParagraphs_DoNotAddExtraParagraph()
    {
        var input = "a\n   \n\nb";

        var html = _md.Render(input).ReplaceLineEndings(string.Empty);

        html.Should().Be("<p>a   </p><p>b</p>");
    }

    [Test]
    public void Render_MultipleEmptyLinesBetweenParagraphs_ProducesTwoParagraphs()
    {
        var input = "a\n\n\n\nb";

        var html = _md.Render(input).ReplaceLineEndings(string.Empty);

        html.Should().Be("<p>a</p><p>b</p>");
    }

    [Test]
    public void Render_ThreeParagraphsWithHeading_ShowsCorrectHtml()
    {
        var input = "_эм_ абзац\n\n# Заголовок\n\ntext __bold__";

        var html = _md.Render(input).ReplaceLineEndings(string.Empty);

        html.Should().Be("<p><em>эм</em> абзац</p><h1>Заголовок</h1><p>text <strong>bold</strong></p>");
    }

    [Test]
    public void Render_EmptyInput_ReturnsEmptyString()
    {
        var html = _md.Render(string.Empty);

        html.Should().Be(string.Empty);
    }

    [Test]
    public void Render_SpacesOnlyParagraph_ShowsEmptyParagraph()
    {
        var input = "   ";

        var html = _md.Render(input);

        html.Should().Be("<p></p>");
    }

    [Test]
    public void Render_EmWithUnicodeChar_ShowsCorrectHtml()
    {
        var input = "текст с _символом unicode ▭_ внутри";

        var html = _md.Render(input);

        html.Should().Be("<p>текст с <em>символом unicode ▭</em> внутри</p>");
    }
}
