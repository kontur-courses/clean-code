using FluentAssertions;
using Markdown;
using System.Diagnostics;
using System.Text;

namespace MarkdownTests;

[TestFixture]
public class MarkdownSpeedTest
{
    [Test]
    public void Render_ShouldWorkFast()
    {
        var sw = new Stopwatch();
        var results = new List<TimeSpan>();

        for (var length = 640; length <= 5120; length *= 2)
        {
            var text = GetRandomString(length);
            sw.Start();
            Md.Render(text);
            sw.Stop();
            results.Add(sw.Elapsed);
            sw.Reset();
        }

        for (var i = 1; i < results.Count; i++)
            (results[i].Ticks / results[i - 1].Ticks).Should().BeLessThan(4);
    }

    private string GetRandomString(int length)
    {
        Random rand = new Random();

        var variants = new List<string>
        {
            " ", "_", "__", "  ", Environment.NewLine, ((char)rand.Next('A', 'Z' + 1)).ToString()
        };
        var text = new StringBuilder();
        var rnd = new Random();
        for (var i = 0; i < length; i++) text.Append(variants[rnd.Next(variants.Count)]);
        return text.ToString();
    }
}