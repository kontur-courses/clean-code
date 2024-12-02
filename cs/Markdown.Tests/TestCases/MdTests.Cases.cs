namespace Markdown.Tests;

public partial class MdTests
{
    private static readonly IEnumerable<TestCaseData> conversionTestCases =
    [
        new TestCaseData(
                "\n* слова \n* в \n* разных \n* тегах",
                "\n<ul><li> слова </li><li> в </li><li> разных </li><li> тега</li></ul>")
            .SetName("SimpleBulletedList")
            .SetCategory("BulletedList"),
        new TestCaseData(
                "* это в одном списке \n \n* это в другом \n",
                "<ul><li> это в одном списке </li></ul> \n<ul><li> это в другом </li></ul>")
            .SetName("DoubleBulletedList")
            .SetCategory("BulletedList"),
        new TestCaseData(
                "# Заголовок с разными символами",
                "<h1>Заголовок с разными символами</h1>")
            .SetName("SimpleHeadline")
            .SetCategory("Headline"),
        new TestCaseData(
                "Текст, _окруженный с двух сторон_ одинарными символами подчерка",
                "Текст, <i>окруженный с двух сторон</i> одинарными символами подчерка")
            .SetName("SimpleItalic")
            .SetCategory("Italic"),
        new TestCaseData(
                "__Выделенный двумя символами текст__ должен становиться полужирным",
                "<b>Выделенный двумя символами текст</b> должен становиться полужирным")
            .SetName("SimpleBold")
            .SetCategory("Bold"),
        new TestCaseData(
                "Символ экранирования тоже можно экранировать: \\\\_вот это будет выделено тегом_",
                "Символ экранирования тоже можно экранировать: \\<i>вот это будет выделено тегом</i>")
            .SetName("DoubleShielding")
            .SetCategory("Shielding"),
        new TestCaseData(
                "Однако выделять часть слова они могут: и в __нач__але",
                "Однако выделять часть слова они могут: и в <b>нач</b>але")
            .SetName("HighlightingBeginningWord")
            .SetCategory("Bold"),
        new TestCaseData(
                "Однако выделять часть слова они могут: и в сер_еди_не,",
                "Однако выделять часть слова они могут: и в сер<i>еди</i>не,")
            .SetName("HighlightingMiddleWord")
            .SetCategory("Italic"),
        new TestCaseData(
                "Однако выделять часть слова они могут: и в кон_це._",
                "Однако выделять часть слова они могут: и в кон<i>це.</i>")
            .SetName("HighlightingEndWord")
            .SetCategory("Italic"),
        new TestCaseData(
                "Внутри __двойного выделения _одинарное_ тоже__ работает.",
                "Внутри <b>двойного выделения <i>одинарное</i> тоже</b> работает.")
            .SetName("NestedTagsItalicInBold")
            .SetCategory("Bold")
            .SetCategory("Italic"),
        new TestCaseData(
                "Но не наоборот — внутри _одинарного __двойное__ не_ работает.",
                "Но не наоборот — внутри <i>одинарного __двойное__ не</i> работает.")
            .SetName("NestedTagsBoldInItalic")
            .SetCategory("Bold")
            .SetCategory("Italic"),
        new TestCaseData(
                "# Заголовок __с _разными_ символами__ \n* и списком \n ",
                "<h1>Заголовок <b>с <i>разными</i> символами</b> </h1><ul><li> и списком </li></ul> ")
            .SetName("OverlappingTags")
            .SetCategory("Bold")
            .SetCategory("Italic")
            .SetCategory("BulletedList")
            .SetCategory("Headline"),
    ];
    
    private static readonly IEnumerable<TestCaseData> noConversionTestCases =
    [
        new TestCaseData(
                "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.",
                "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.")
            .SetName("OverlappingTags")
            .SetCategory("Bold")
            .SetCategory("Italic"),
        new TestCaseData(
                "\\_Вот это\\_, не должно выделиться тегом и должен остаться второй символ экранирования",
                "_Вот это\\_, не должно выделиться тегом и должен остаться второй символ экранирования")
            .SetName("TwoShielding")
            .SetCategory("Italic"),
        new TestCaseData(
                "\\_Вот это_, не должно выделиться тегом и должен остаться второй символ экранирования",
                "_Вот это_, не должно выделиться тегом и должен остаться второй символ экранирования")
            .SetName("OneShielding")
            .SetCategory("Italic"),
        new TestCaseData(
                "Здесь сим\\волы экранирования\\ \\должны остаться.\\",
                "Здесь сим\\волы экранирования\\ \\должны остаться.\\")
            .SetName("DoubleShielding"),
        new TestCaseData(
                "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.",
                "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.")
            .SetName("BetweenInDigits")
            .SetCategory("Italic"),
        new TestCaseData(
                "Подчерки внутри текста c цифрами__12__3 не считаются выделением и должны оставаться символами подчерка.",
                "Подчерки внутри текста c цифрами__12__3 не считаются выделением и должны оставаться символами подчерка.")
            .SetName("BetweenInDigits")
            .SetCategory("Bold"),
        new TestCaseData(
                "__Непарные_ символы в рамках одного абзаца не считаются выделением.",
                "__Непарные_ символы в рамках одного абзаца не считаются выделением.")
            .SetName("UnpairedUnderscores")
            .SetCategory("Italic")
            .SetCategory("Bold"),
        new TestCaseData(
                "Иначе эти_ подчерки_ не считаются выделением",
                "Иначе эти_ подчерки_ не считаются выделением")
            .SetName("EmptyAfterStartTag")
            .SetCategory("Italic"),
        new TestCaseData(
                "Иначе эти __подчерки __не считаются _окончанием выделения",
                "Иначе эти __подчерки __не считаются _окончанием выделения")
            .SetName("EmptyBeforeEndTag")
            .SetCategory("Bold"),
    ];
}