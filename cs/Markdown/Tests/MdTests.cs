using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[NonParallelizable]
public class MdTests
{
    [TestCaseSource(nameof(ConvertToHtml))]
    [TestCaseSource(nameof(IgnoreConvert))]
    public void Render_Text_ShouldBeCorrect(string input, string expectedOutput)
    {
        var actual = Md.Render(input);
        
        actual.Should().Be(expectedOutput, $"\nInput: {input}\nActual: {actual}\nExpected:{expectedOutput}");
    }

    private static TestCaseData[] ConvertToHtml() =>
    [
        new TestCaseData("_окруженный с двух сторон_", "<em>окруженный с двух сторон</em>").SetName("Text_WithEmTag_ConvertToHtml"),
        new TestCaseData("__Выделенный двумя символами текст__", "<strong>Выделенный двумя символами текст</strong>").SetName("Text_WithStrongTag_ConvertToHtml"),
        new TestCaseData(@"\\_вот это будет выделено тегом_", @"\<em>вот это будет выделено тегом</em>").SetName("Text_WithEmTagAndEscapedBackslash_ConvertToHtml"),
        new TestCaseData("__двойного выделения _одинарное_ тоже__", "<strong>двойного выделения <em>одинарное</em> тоже</strong>").SetName("Text_WithEmTagInStrongTag_ConvertToHtml"),
        new TestCaseData("Но не наоборот — внутри _одинарного __двойное__ не_ работает",
            "Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает").SetName("Text_WithStrongTagInEmTag_ConvertToHtml"),
        new TestCaseData("_12_3", "<em>12</em>3").SetName("Word_WithEmTagInNumber_ConvertToHtml"),
        new TestCaseData("_нач_але, и в сер_еди_не, и в кон_це._", "<em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>").SetName("Text_WithEmTagInDifferentOneWordParts_ConvertToHtml"),
        new TestCaseData("# Заголовок __с _разными_ символами__",
            "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>").SetName("Text_WithDifferentTags_ConvertToHtml"),
        new TestCaseData("ра_зных _сл_овах", "ра_зных <em>сл</em>овах").SetName("Text_WithUnderlineAndEmTagInOtherWord_ConvertToHtml"),
        new TestCaseData("# Заголовки\n", "<h1>Заголовки</h1>\n").SetName("Text_WithH1AndNewline_ConvertToHtml"),
        new TestCaseData("_ff _ ff_", "<em>ff _ ff</em>").SetName("Text_WithEmTagAndUnderline_ConvertToHtml"),
        new TestCaseData("[l](https://yandex.ru/ \"F\")", "<a href=\"https://yandex.ru/\" title=\"F\">l</a>").SetName("Text_LinkWithComment_ConvertToHtml"),
        new TestCaseData("[l](https://yandex.ru/)", "<a href=\"https://yandex.ru/\">l</a>").SetName("Text_WithSimpleLink_ConvertToHtml"),
        new TestCaseData("# Заголовок _[l](https://yandex.ru/)_",
            "<h1>Заголовок <em><a href=\"https://yandex.ru/\">l</a></em></h1>").SetName("Text_WithMoreDifferentTags_ConvertToHtml"),
        new TestCaseData("[](https://yandex.ru/)", "<a href=\"https://yandex.ru/\"></a>").SetName("Text_WithLinkEmptyWord_ConvertToHtml"),
        new TestCaseData(@"[\\](https://yandex.ru/)", "<a href=\"https://yandex.ru/\">\\\\</a>").SetName("Text_WittBackslashInLinkWord_ConvertToHtml"),
        new TestCaseData(@"\__жирный_", "_<em>жирный</em>").SetName("Text_WithFakeStrongTag_ConvertToHtml"),
        new TestCaseData("# заго#ловок", "<h1>заго#ловок</h1>").SetName("Text_WithFakeH1Tag2_ConvertToHtml"),
        new TestCaseData("__непонятным тек_стом__", "<strong>непонятным тек_стом</strong>").SetName("Text_WithStrongTagWithUnderline_ConvertToHtml"),
    ];

    
    private static TestCaseData[] IgnoreConvert() =>
    [
        new TestCaseData(@"\_Вот это\_", "_Вот это_").SetName("Text_WithEmTagAndEscaping_IgnoreConvert"),
        new TestCaseData(@"Здесь сим\волы экранирования\ \должны остаться.\", @"Здесь сим\волы экранирования\ \должны остаться.\").SetName("Text_WithBackslash_IgnoreConvert"),
        new TestCaseData("цифрами_12_3", "цифрами_12_3").SetName("Word_WithUnderlineBetweenLetterAndDigit_IgnoreConvert"),
        new TestCaseData("ра_зных сл_овах", "ра_зных сл_овах").SetName("Text_WithUnderlineInDifferentWords_IgnoreConvert"),
        new TestCaseData("__Непарные_ символы", "__Непарные_ символы").SetName("Text_WithUnpairTags_IgnoreConvert"),
        new TestCaseData("эти_ подчерки_ не считаются выделением", "эти_ подчерки_ не считаются выделением").SetName("Text_WithOnlyClosingUnderline(OnlyInWordsEnd)_IgnoreConvert"),
        new TestCaseData("эти _подчерки _не считаются окончанием", "эти _подчерки _не считаются окончанием").SetName("Text_WithOnlyOpeningUnderline(OnlyInWordsBegin)_IgnoreConvert"),
        new TestCaseData("__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_").SetName("Text_WithIntersectingTagsInOneWord_IgnoreConvert"),
        new TestCaseData("__пересечения _двойных и__ одинарных_", "__пересечения _двойных и__ одинарных_").SetName("Text_WithIntersectingTagsInDifferentWords_IgnoreConvert"),
        new TestCaseData("___пересечения___", "___пересечения___").SetName("Word_WithIntersectingTagsInOneWord_IgnoreConvert"),
        new TestCaseData("Если внутри подчерков пустая строка ____", "Если внутри подчерков пустая строка ____").SetName("Text_WithEmptyTags_IgnoreConvert"),
        new TestCaseData("[]()", "[]()").SetName("Text_WithEmptyLink_IgnoreConvert"),
        new TestCaseData(@"\[l](https://yandex.ru/)", "[l](https://yandex.ru/)").SetName("Text_WithEscaping[_IgnoreConvert"),
        new TestCaseData(@"[l\](https://yandex.ru/)", "[l](https://yandex.ru/)").SetName("Text_WithEscaping]_IgnoreConvert"),
        new TestCaseData(@"[l]\(https://yandex.ru/)", @"[l]\(https://yandex.ru/)").SetName("Text_WithSymbolBetweenLinkParts_IgnoreConvert"),
        new TestCaseData("Не # заголовок", "Не # заголовок").SetName("Text_WithFakeH1Tag1_IgnoreConvert"),
        new TestCaseData("#! Не съедай символ!", "#! Не съедай символ!").SetName("Text_WithH1MissingSymbol_IgnoreConvert"),
        new TestCaseData("__ой __", "__ой __").SetName("Text_WithFakeStrongTag_IgnoreConvert")
    ];
    
    [Test]
    public void Render_ShouldWorkWithLinearComplexity()
    {
        var textFragment = "# __TEST__";
        var textTimeRenders = new List<TimeSpan>();
        var rendersNumber = 14;
        var expected = (int)(14 * 0.6);
        
        for (var i = 0; i <= rendersNumber; i++)
        {
            var input = GetNLengthsStringFromFragment(textFragment, Math.Pow(2, i));
            WarmupMdParser();
            textTimeRenders.Add(GetRenderTime(input));
        }
        var actual = GetActualResult(textTimeRenders);
        
        actual.Should().BeGreaterThanOrEqualTo(expected);
    }
    
    private static void WarmupMdParser() => Md.Render(" ");

    private static string GetNLengthsStringFromFragment(string textFragment, double length)
    {
        var inputBuilder = new StringBuilder();
        for (var i = 0; i < length; i++)
            inputBuilder.Append(textFragment);
        return inputBuilder.ToString();
    }
    
    private static TimeSpan GetRenderTime(string input)
    {
        var firstTimer = new Stopwatch();
        firstTimer.Start();
        Md.Render(input);
        firstTimer.Stop();
        return firstTimer.Elapsed;
    }

    private static int GetActualResult(List<TimeSpan> textTimeRenders)
    {
        var result = 0;
        for (var i = 0; i < textTimeRenders.Count - 1; i++)
            if (textTimeRenders[i + 1] / textTimeRenders[i] < 2)
                result += 1;
        return result;
    }
}
