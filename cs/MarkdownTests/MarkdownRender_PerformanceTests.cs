using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    //TODO add tests
    public class MarkdownRender_PerformanceTests
    {
        [Test]
        public void Render_ShouldWorkFast()
        {
            var sw = new Stopwatch();
            var results = new List<TimeSpan>();
            var mdRender = new MarkdownRenderer(new HtmlTranslator());
            for (var length = 64000; length <= 512 * 1000; length *= 2)
            {
                var text = GetRandomString(length);
                sw.Start();
                mdRender.Render(text);
                sw.Stop();
                results.Add(sw.Elapsed);
                sw.Reset();
            }

            for (var i = 1; i < results.Count; i++)
                (results[i].Ticks / results[i - 1].Ticks).Should().BeLessThan(4);
        }

        private string GetRandomString(int length)
        {
            var variants = new List<string>
            {
                " ", "_", "__", "\\", "[", "]", "(", ")", "  ", Environment.NewLine, "!"
            };
            var text = new StringBuilder();
            var rnd = new Random();
            for (var i = 0; i < length; i++) text.Append(variants[rnd.Next(variants.Count)]);
            return text.ToString();
        }
    }
}