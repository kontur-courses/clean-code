using System.Diagnostics;
using System.Text;
using FluentAssertions;
using static Markdown.Md;
using TimeSpan = System.TimeSpan;

namespace MarkdownTest;

[TestFixture]
public class MarkdownPerformanceTests
{
    private static readonly (int Start, int Finish) AsciiSymbolsBorder = new (32, 126);

    [Test]
    [Explicit]
    [Repeat(100)]
    public void Markdown_Render_ShouldWorkFastThanNLogN()
    {
        const int scale = 10;
        var sw = new Stopwatch();
        var timeSpans = new List<TimeSpan>();
        
        Render(GenerateRandomMarkdown(1000));
        
        for (var length = 10; length <= 1000000; length *= scale)
        {
            var markdown = GenerateRandomMarkdown(length);
            sw.Start();
            Render(markdown);
            GC.Collect();
            sw.Stop();
            timeSpans.Add(sw.Elapsed);
            sw.Reset();
        }

        var timeRatios = Enumerable.Range(0, timeSpans.Count - 2)
            .Select(i => (double)timeSpans[i + 1].Ticks / timeSpans[i].Ticks);

        var maxAllowedRatio = scale * Math.Log(scale);
        
        timeRatios.Should()
            .OnlyContain(timeRatio => timeRatio < maxAllowedRatio);
    }

    private static string GenerateRandomMarkdown(int len)
    {
        var elements = Enumerable
            .Range(AsciiSymbolsBorder.Start, AsciiSymbolsBorder.Finish - AsciiSymbolsBorder.Start + 1)
            .Select(c => ((char)c).ToString())
            .ToList();
        var sb = new StringBuilder();
        
        for (var i = 0; i < len; i++)
            sb.Append(elements[Random.Shared.Next(elements.Count)]);

        return sb.ToString();
    }
}