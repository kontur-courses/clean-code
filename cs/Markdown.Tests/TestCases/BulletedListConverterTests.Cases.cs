using Markdown.Converters;
using Markdown.Models;

namespace Markdown.Tests.ConverterTests;

public partial class BulletedListConverterTests
{
    private const string markdownTagStart = "*";
    private const string markdownTagEnd = "\n";
    
    private const string htmlHeadStartTag = "<ul>";
    private const string htmlHeadEndTag = "</ul>";

    private const string htmlTagStart = "<li>";
    private const string htmlTagEnd = "</li>";

    private static readonly IEnumerable<TestCaseData> conversionTestCases =
    [
        new TestCaseData(
                $"* Элемент с \\n в конце \n",
                new[]
                {
                    TagPair
                        .CreateBuilder(typeof(BulletedListConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlHeadStartTag + htmlTagStart, 0))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd + htmlHeadEndTag, 23))
                        .Build(),
                })
            .SetName("Default convert with \\n"),
        new TestCaseData(
                $"* Элемент \n* Элемент \n* Элемент ",
                new[]
                {
                    TagPair
                        .CreateBuilder(typeof(BulletedListConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlHeadStartTag + htmlTagStart, 0))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 10))
                        .Build(),
                    TagPair
                        .CreateBuilder(typeof(BulletedListConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 11))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 21))
                        .Build(),
                    TagPair
                        .CreateBuilder(typeof(BulletedListConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 22))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd + htmlHeadEndTag, 31))
                        .Build(),
                })
            .SetName("Default convert without \\n in end"),
        new TestCaseData(
                $"* Элемент 1\n* Элемент 2\n* Элемент 3 \n \n* Элемент 1\n* Элемент 2\n* Элемент 3",
                new[]
                {
                    TagPair
                        .CreateBuilder(typeof(BulletedListConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlHeadStartTag + htmlTagStart, 0))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 11))
                        .Build(),
                    TagPair
                        .CreateBuilder(typeof(BulletedListConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 12))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 23))
                        .Build(),
                    TagPair
                        .CreateBuilder(typeof(BulletedListConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 24))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd + htmlHeadEndTag, 36))
                        .Build(),
                    TagPair
                        .CreateBuilder(typeof(BulletedListConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlHeadStartTag +  htmlTagStart, 39))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 50))
                        .Build(),
                    TagPair
                        .CreateBuilder(typeof(BulletedListConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 51))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 62))
                        .Build(),
                    TagPair
                        .CreateBuilder(typeof(BulletedListConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 63))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd + htmlHeadEndTag, 73))
                        .Build(),
                })
            .SetName("Double convert"),
    ];
    private static readonly IEnumerable<TestCaseData> noConversionTestCases =
    [
        new TestCaseData(string.Empty, Array.Empty<TagPair>())
            .SetName("EmptyInputString"),
        new TestCaseData(
                $"тут какой то текст * Элемент \n",
                Array.Empty<TagPair>())
            .SetName("TextInStart"),
    ];
}