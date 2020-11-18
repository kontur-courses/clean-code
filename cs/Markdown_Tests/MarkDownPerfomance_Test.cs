using System.Diagnostics;
using System.IO;
using System.Text;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace Markdown_Tests
{
    public class MarkDownPerfomance_Test
    {
        [Test, Timeout(30000)]
        public void PerformanceTest()
        {
            var currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent;
            
            var textFromFile = new StreamReader($"{currentDirectory.FullName}\\MarkdownSpec.md")
                .ReadToEnd();
            var mdToHtml = new Md();
            var stopwatch = Stopwatch.StartNew();
            mdToHtml.Render(textFromFile);
            var firstStartTime = stopwatch.Elapsed;

            var stringBuilder = new StringBuilder();
            for (var i = 0; i < 4; i++)
            {
                var text = stringBuilder.Append(textFromFile.Substring(i * 25, 25)).ToString();
                stopwatch = Stopwatch.StartNew();
                mdToHtml.Render(text);
                var currentTime = stopwatch.Elapsed;
                currentTime.Should().BeLessThan((i + 1) * firstStartTime * 10);
            }
        }
    }
}