using Markdown.Converters;
using Markdown.Models;

namespace Markdown.Tests.ConverterTests;

public partial class HeaderConverterTests
{
    private const string markdownTagStart = "# ";
    private const string markdownTagEnd = "\n";
    private const string htmlTagStart = "<h1>";
    private const string htmlTagEnd = "</h1>";

    private static readonly IEnumerable<TestCaseData> noConversionTestCases =
    [
        new TestCaseData(string.Empty, Array.Empty<TagPair>())
            .SetName("EmptyInputString"),
    ];

    private static readonly IEnumerable<TestCaseData> conversionTestCases =
    [
        new TestCaseData($"{markdownTagStart}Заголовок",
                new[]
                {
                    TagPair
                        .CreateBuilder(typeof(HeaderConverter))
                        .AddOpenToken(new TagToken(markdownTagStart, htmlTagStart, 0))
                        .AddCloseToken(new TagToken(markdownTagEnd, htmlTagEnd, 11))
                        .Build(),
                })
            .SetName("BasicHeader"),
    ];
}