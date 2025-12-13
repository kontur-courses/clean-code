using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MdPerformanceTest
{   
    [Test]
    public void Render_Performance_WithAllTokens()
    {
        var repeatCounts = new List<int>
        {
            1000,
            2000,
            4000,
            8000,
            16000
        };
        const string markdownText = "# Заголовок _курсив_ __жирный__ ![Cat](https://example.com/image.jpg) текст" +
                                    " \\экранирование_\n";
        long? baseTimeMs = null;
        var baseRepeatCount = repeatCounts[0];

        foreach (var repeatCount in repeatCounts)
        {
            var sb = new StringBuilder();
            var md = new Md();
            for (var i = 0; i < repeatCount; i++)
                sb.Append(markdownText);
            var input = sb.ToString();
            
            var stopwatch = Stopwatch.StartNew();
            md.Render(input);
            stopwatch.Stop();

            var elapsedMs = stopwatch.ElapsedMilliseconds;
            if (baseTimeMs == null)
            {
                baseTimeMs = elapsedMs;
                continue;
            }

            var multiplier = repeatCount / baseRepeatCount;
            var expectedTime = baseTimeMs.Value * multiplier;
            const double tolerance = 0.20;
            var max = (long)(expectedTime * (1 + tolerance));
            elapsedMs.Should().BeLessThanOrEqualTo(max);
        }
    }
}