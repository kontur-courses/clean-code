using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_RenderingToHtml_PerformanceTests
    {
        private readonly Md md = new Md();

        [Test]
        public void RenderNonNestedTagsByLinearTime()
        {
            const string template = @"a __bb__ ccc _d_ \_e\_";
            var timer = new Stopwatch();
            var results = new List<double>();
            var accuracyCoefficient = 1.5;

            var minRepeatCount = 1000;
            var maxRepeatCount = 1000000;
            var step = 10;
            for (int repeatsCount = minRepeatCount; repeatsCount <= maxRepeatCount; repeatsCount *= step)
            {
                var currentText = string.Concat(Enumerable.Repeat(template, repeatsCount));
                GC.Collect();

                timer.Start();
                md.RenderToHtml(currentText);
                timer.Stop();

                results.Add((double)timer.ElapsedMilliseconds / repeatsCount);
                timer.Reset();
            }
            var maxValue = results[0] * accuracyCoefficient;

            results.All(v => v <= maxValue).Should().BeTrue();
        }

        [Test]
        public void RenderNestedTagsByLinearTime()
        {
            var timer = new Stopwatch();
            var results = new List<double>();
            var accuracyCoefficient = 1.5;

            var minCharsCount = 10000;
            var maxCharsCount = 10000000;
            var step = 10;
            for (int charsCount = minCharsCount; charsCount <= maxCharsCount; charsCount *= step)
            {
                var currentText = "__b _" + new string('a', charsCount) + "_ b__";
                GC.Collect();

                timer.Start();
                md.RenderToHtml(currentText);
                timer.Stop();

                results.Add((double)timer.ElapsedMilliseconds / charsCount);
                timer.Reset();
            }
            var maxValue = results[0] * accuracyCoefficient;

            results.All(v => v <= maxValue).Should().BeTrue();
        }
    }
}