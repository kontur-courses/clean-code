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
            var res = new List<TimeSpan>();
            var render = new HtmlRender();
            for (var i = 64000; i <= 2048 * 1000; i *= 2)
            {
                var text = GetRandomString(i);
                sw.Start();
                MarkdownRender.Render(render, text);
                sw.Stop();
                res.Add(sw.Elapsed);
                sw.Reset();
            }

            for (var i = 1; i < res.Count; i++)
                (res[i].Ticks / res[i - 1].Ticks).Should().BeLessThan(4);
        }

        private string GetRandomString(int length)
        {
            var variants = new List<string>()
            {
                " ", "_", "__", "\\", "# ", "#", "a", "b", "c", "d"
            };
            var text = new StringBuilder();
            var rnd = new Random();
            for (var i = 0; i < length; i++)
            {
                text.Append(variants[rnd.Next(variants.Count)]);
            }
            return text.ToString();
        }
    }
}