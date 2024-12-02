using Markdown.Converters;
using Markdown.Models;

namespace Markdown.Tests.ConverterTests;

internal partial class ItalicConverterTests
{
    private const string markdownTagStart = "_";
    private const string markdownTagEnd = "_";
    private const string htmlTagStart = "<i>";
    private const string htmlTagEnd = "</i>";

    private static readonly IEnumerable<TestCaseData> noConversionTestCases =
    [
        new TestCaseData(string.Empty, Array.Empty<TagPair>())
            .SetName("EmptyInputString"),
        new TestCaseData(
                $"Подчерки внутри текста c цифрами{markdownTagStart}12{markdownTagEnd}3 не считаются выделением и должны оставаться символами подчерка.",
                Array.Empty<TagPair>())
            .SetName("WithDigits"),
        new TestCaseData($"Выделение в ра{markdownTagStart}зных сл{markdownTagEnd}овах не работает.",
                Array.Empty<TagPair>())
            .SetName("AcrossWords"),
        new TestCaseData($"__Непарные{markdownTagEnd} символы в рамках одного абзаца не считаются выделением.",
                Array.Empty<TagPair>())
            .SetName("UnpairedTagsInEnd"),
        new TestCaseData($"{markdownTagStart}Непарные__ символы в рамках одного абзаца не считаются выделением.",
                Array.Empty<TagPair>())
            .SetName("UnpairedTagsInStart"),
        new TestCaseData("За подчерками, начинающими выделение, должен следовать непробельный символ. " +
                         $"Иначе эти{markdownTagStart} подчерки{markdownTagEnd} не считаются выделением и остаются просто символами подчерка.",
                Array.Empty<TagPair>())
            .SetName("SpaceAfterOpeningTag"),
        new TestCaseData("Подчерки, заканчивающие выделение, должны следовать за непробельным символом. " +
                         $"Иначе эти {markdownTagStart}подчерки {markdownTagEnd}не считаются окончанием выделения и остаются просто символами подчерка.",
                Array.Empty<TagPair>())
            .SetName("SpaceBeforeClosingTag"),
        new TestCaseData(
                $"Если внутри подчерков пустая строка {markdownTagStart}{markdownTagEnd}{markdownTagStart}{markdownTagEnd}, то они остаются символами подчерка.",
                Array.Empty<TagPair>())
            .SetName("WithoutContent"),
    ];

    private static readonly IEnumerable<TestCaseData> conversionTestCases =
    [
        new TestCaseData(
                $"{markdownTagStart}Выделенный двумя символами текст{markdownTagEnd} должен становиться полужирным с помощью тега",
                new[]
                {
                    TagPair
                        .CreateBuilder(typeof(ItalicConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 0))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 33))
                        .Build(),
                })
            .SetName("Default convert"),
        new TestCaseData($"Однако выделять часть слова они могут: и в {markdownTagStart}нач{markdownTagEnd}але,",
                new[]
                {
                    TagPair
                        .CreateBuilder(typeof(ItalicConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 43))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 47))
                        .Build(),
                })
            .SetName("BeginningPartialWordConversion"),
        new TestCaseData($"Однако выделять часть слова они могут: и в сер{markdownTagStart}еди{markdownTagEnd}не",
                new[]
                {
                    TagPair
                        .CreateBuilder(typeof(ItalicConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 46))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 50))
                        .Build(),
                })
            .SetName("MiddlePartialWordConversion"),
        new TestCaseData($"Однако выделять часть слова они могут: и в кон{markdownTagStart}це.{markdownTagEnd}",
                new[]
                {
                    TagPair
                        .CreateBuilder(typeof(ItalicConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 46))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 50))
                        .Build(),
                })
            .SetName("EndPartialWordConversion"),

        new TestCaseData("Подчерки, заканчивающие выделение, должны следовать за непробельным символом. " +
                         $"Иначе эти {markdownTagStart}подчерки {markdownTagStart}не считаются{markdownTagEnd} окончанием выделения и остаются просто символами подчерка," +
                         "но после невалидных тегов могут идти валидные",
                new[]
                {
                    TagPair
                        .CreateBuilder(typeof(ItalicConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 98))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 111))
                        .Build(),
                })
            .SetName("AfterInvalidTag"),
    ];
}