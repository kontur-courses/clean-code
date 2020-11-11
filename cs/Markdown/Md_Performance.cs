using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Markdown
{
    [TestFixture]
    public class Md_Performance
    {
        [Test]
        [Timeout(25000)]
        public void Should_WorkLinear()
        {
            var timesOfWork = new List<double>();
            for (var count = 1000; count <= 10000; count+=1000)
            {
                var timeOfWork = Should_CorrectParse_MarkdownSpec(count);
                Console.WriteLine($"{count} {timeOfWork}");
                timesOfWork.Add(timeOfWork.Ticks);
            }

            var expectedTime = timesOfWork[0] * 2;
            for (var i = 1; i < timesOfWork.Count; i++)
            {
                timesOfWork[i].Should().BeLessOrEqualTo(expectedTime * 1.5);

                expectedTime = Math.Max(timesOfWork[i], expectedTime) * (i + 1.0) / i;
            }
        }
        
        public static TimeSpan Should_CorrectParse_MarkdownSpec(int count = 1)
        {
            var markdownDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent;
            var rootDirectory = markdownDirectory?.Parent?.Parent;
            if (rootDirectory == null) return new TimeSpan();
            
            using var mdReader = new StreamReader($"{rootDirectory.FullName}\\MarkdownSpec.md");
            var md = DuplicateLine(mdReader.ReadToEnd(), count);
            
            using var htmlReader = new StreamReader($"{markdownDirectory.FullName}\\MarkdownSpecExpected.html");
            var expectedHtml = DuplicateLine(htmlReader.ReadToEnd(), count);
            
            var stopwatch = Stopwatch.StartNew();
            var actualHtml = Md.Render(md);
            var result = stopwatch.Elapsed;
            
            actualHtml.Should().Be(expectedHtml);

            if (count != 1 || TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return result;
            using var writer = new StreamWriter($"{rootDirectory.FullName}\\MarkdownSpecSample.html");
            writer.Write(actualHtml);

            return result;
        }

        private static string DuplicateLine(string source, int count)
        {
            if (count <= 0) throw new ArgumentException();
            
            var builder = new StringBuilder();
            for (var i = 0; i < count; i++) 
                builder.AppendLine(source);
            
            return builder.ToString();
        }
    }
}