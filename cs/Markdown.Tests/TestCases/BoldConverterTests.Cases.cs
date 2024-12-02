using Markdown.Converters;
using Markdown.Models;

namespace Markdown.Tests.ConverterTests;

internal partial class BoldConverterTests
{
    private const string markdownTagStart = "__";
    private const string markdownTagEnd = "__";
    private const string htmlTagStart = "<b>";
    private const string htmlTagEnd = "</b>";
    
    private static readonly IEnumerable<TestCaseData> noConversionTestCases =
    [
        new TestCaseData(string.Empty, Array.Empty<TagPair>())
            .SetName("EmptyInputString"),
        new TestCaseData(
                $"Подчерки внутри текста c цифрами{markdownTagStart}12{markdownTagEnd}3 не считаются выделением и должны оставаться символами подчерка.", 
                Array.Empty<TagPair>())
            .SetName("WithDigits"),
        new TestCaseData($"Выделение в ра{markdownTagStart}ных сл{markdownTagEnd}овах не работает.", Array.Empty<TagPair>())
            .SetName("AcrossWords"),
        new TestCaseData($"{markdownTagStart}Непарные_ символы в рамках одного абзаца не считаются выделением.", Array.Empty<TagPair>())
            .SetName("UnpairedTagsInEnd"),
        new TestCaseData($"_Непарные{markdownTagEnd} символы в рамках одного абзаца не считаются выделением.", Array.Empty<TagPair>())
            .SetName("UnpairedTagsInStart"),
        new TestCaseData("За подчерками, начинающими выделение, должен следовать непробельный символ. " +
                         $"Иначе эти{markdownTagStart} подчерки{markdownTagEnd} не считаются выделением и остаются просто символами подчерка.", 
                Array.Empty<TagPair>())
            .SetName("SpaceAfterOpeningTag"),
        new TestCaseData("Подчерки, заканчивающие выделение, должны следовать за непробельным символом. " +
                         $"Иначе эти {markdownTagStart}подчерки {markdownTagEnd}не считаются окончанием выделения и остаются просто символами подчерка.", 
                Array.Empty<TagPair>())
            .SetName("SpaceBeforeClosingTag"),
        new TestCaseData($"Если внутри подчерков пустая строка {markdownTagStart}{markdownTagEnd}, то они остаются символами подчерка.",
                Array.Empty<TagPair>())
            .SetName("WithoutContent"),
    ];

    private static readonly IEnumerable<TestCaseData> conversionTestCases =
    [
        new TestCaseData($"{markdownTagStart}Выделенный двумя символами текст{markdownTagEnd} должен становиться полужирным с помощью тега", 
                new[] 
                {
                    TagPair
                        .CreateBuilder(typeof(BoldConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 0))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 34))
                        .Build(),
                })
            .SetName("Default convert"),
        new TestCaseData($"Однако выделять часть слова они могут: и в {markdownTagStart}нач{markdownTagEnd}але,", 
                new[] 
                {
                    TagPair
                        .CreateBuilder(typeof(BoldConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 43))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 48))
                        .Build(),
                })
            .SetName("BeginningPartialWordConversion"),
        new TestCaseData($"Однако выделять часть слова они могут: и в сер{markdownTagStart}еди{markdownTagEnd}не", 
                new[] 
                {
                    TagPair
                        .CreateBuilder(typeof(BoldConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 46))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 51))
                        .Build(),
                })
            .SetName("MiddlePartialWordConversion"),
        new TestCaseData($"Однако выделять часть слова они могут: и в кон{markdownTagStart}це.{markdownTagEnd}", 
                new[] 
                {
                    TagPair
                        .CreateBuilder(typeof(BoldConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 46))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 51))
                        .Build(),
                })
            .SetName("EndPartialWordConversion"),
        
        new TestCaseData("Подчерки, заканчивающие выделение, должны следовать за непробельным символом. " +
                         $"Иначе эти {markdownTagStart}подчерки {markdownTagStart}не считаются{markdownTagEnd} окончанием выделения и остаются просто символами подчерка," +
                         "но после невалидных тегов могут идти валидные", 
                new[] 
                {
                    TagPair
                        .CreateBuilder(typeof(BoldConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 99))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 113))
                        .Build(),
                })
            .SetName("AfterInvalidTag"),
    ];
}