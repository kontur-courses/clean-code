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
        [Timeout(20000)]
        public void Should_WorkLinear()
        {
            var timesOfWork = new List<long>();
            for (var count = 1000; count <= 10000; count+=1000)
            {
                var timeOfWork = ParseMarkdownSpec(count);
                Console.WriteLine($"{count} {timeOfWork}");
                timesOfWork.Add(timeOfWork.Ticks);
            }

            for (var i = 1; i < timesOfWork.Count; i++)
            {
                var timeIncrease = (double)timesOfWork[i] / timesOfWork[i - 1];
                var lengthIncrease = (i + 1.0) / i;
                timeIncrease.Should().BeLessOrEqualTo(lengthIncrease * 1.2);
            }
        }
        
        public static TimeSpan ParseMarkdownSpec(int count = 1)
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