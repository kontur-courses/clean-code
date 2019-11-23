using System;
using System.Diagnostics;
using FluentAssertions;
using Markdown.Properties;
using NUnit.Framework;

namespace Markdown.Core.Tests
{
    [TestFixture]
    public class MarkdownProcessorPerformanceTests
    {
        private MarkdownProcessor processor;

        [SetUp]
        public void SetUp()
        {
            processor = MarkdownProcessorFactory.CreateMarkdownToHtmlProcessor();
        }

        [Test]
        public void Render_ShouldRenderFast()
        {
            var averageFileData = Resources.MarkdownTestFileAverage;
            var bigFileData = Resources.MarkdownTestFileBig;

            var averageFileProcessingTime = MeasureRunTimeInMilliseconds(() => processor.Render(averageFileData));
            var bigFileProcessingTime = MeasureRunTimeInMilliseconds(() => processor.Render(bigFileData));

            bigFileProcessingTime.Should()
                .BeLessOrEqualTo(bigFileData.Length / averageFileData.Length * averageFileProcessingTime);
        }

        private long MeasureRunTimeInMilliseconds(Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            action();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}