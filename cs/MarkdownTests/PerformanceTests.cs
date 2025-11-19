using FluentAssertions;
using Markdown;
using Markdown.Inlines;
using System.Diagnostics;
using System.Text;

namespace MarkdownTests;

[TestFixture]
public class PerformanceTests
{
    private static readonly string[] Patterns =
    [
        "# Заголовок __с _разными_ символами__\n\n",
        "Текст, _окруженный_ и __сильный__ и \\_экранированный\\_ 12_3 ",
        "слово ра_зных сл_овах не работает. __Непарные_ и _непарные__ . ",
        "Внутри __двойного _одинарное_ тоже__ работает. ",
        "Но _внутри __одинарного__ не_ работает. Конец\\\\\n\n"
    ];

    private const double AllowedGrowth = 2.0;
    private const int NestingLevels = 1000;

    private Md _md = null!;

    [SetUp]
    public void SetUp()
    {
        _md = new Md(new BlockSegmenter(), new InlineParser(), new HtmlRenderer());
    }

    private static string BuildText(int size)
    {
        var sb = new StringBuilder(size);
        var patternIndex = 0;

        while (sb.Length < size)
        {
            var pattern = Patterns[patternIndex % Patterns.Length];
            sb.Append(pattern);
            patternIndex++;
        }

        return sb.ToString();
    }

    [Test]
    public void Render_TextSizeIncreases_ExecutionTimeLinearlyOrBetter()
    {
        var sizes = new[] { 2_000, 16_000, 128_000, 1_000_000 };
        var timesMs = new double[sizes.Length];

        const int iterationsPerSize = 5;

        for (int i = 0; i < sizes.Length; i++)
        {
            var size = sizes[i];
            var totalMs = 0.0;

            for (int _ = 0; _ < iterationsPerSize; _++)
            {
                var text = BuildText(size);

                var sw = Stopwatch.StartNew();
                var html = _md.Render(text);
                sw.Stop();

                html.Should().NotBeNullOrEmpty();

                totalMs += sw.Elapsed.TotalMilliseconds;
            }

            timesMs[i] = totalMs / iterationsPerSize;
        }

        var costPerChar = new double[sizes.Length];
        for (int i = 0; i < sizes.Length; i++)
            costPerChar[i] = timesMs[i] / sizes[i];

        for (int i = 1; i < sizes.Length; i++)
            (costPerChar[i] / costPerChar[i - 1]).Should().BeLessThanOrEqualTo(AllowedGrowth);
    }

    [Test]
    public void Render_DeeplyNestedMarkup_DoesNotThrow()
    {
        var builder = new StringBuilder();

        for (int i = 0; i < NestingLevels; i++)
            builder.Append("__вложенный _текст ");

        builder.Append("конец");

        for (int i = 0; i < NestingLevels; i++)
            builder.Append("_ конец__");

        var input = builder.ToString();

        Action act = () => _md.Render(input);

        act.Should().NotThrow();
    }
}
