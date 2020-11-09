using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_Performance
    {
        [Test]
        [Timeout(10000)]
        public void Should_WorkLinear()
        {
            var mdBuilder = new StringBuilder();
            var expectedBuilder = new StringBuilder();
            var timesOfWork = new List<double>();
            for(var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 20000; j++)
                {
                    mdBuilder.Append("#_abc_d __ab cd ef__\r\n_abc __de__ gf_\r\n__ab _cd__ ge_\r\n");
                    expectedBuilder.Append(
                        "<h1><em>abc</em>d <strong>ab cd ef</strong></h1>\r\n<em>abc __de__ gf</em>\r\n__ab _cd__ ge_\r\n");
                }

                var md = mdBuilder.ToString();
                var expected = expectedBuilder + "\r";
                var stopwatch = Stopwatch.StartNew();
                var actual = Md.Render(md);
                var time = stopwatch.Elapsed;
                Console.WriteLine($"{(i + 1) * 60000} lines {time}");
                timesOfWork.Add(time.Ticks);

                actual.Should().Be(expected);
            }

            for (var i = 1; i < timesOfWork.Count; i++)
            {
                var timeIncrease = timesOfWork[i] / timesOfWork[i - 1];
                var lengthIncrease = (i + 1.0) / i;
                timeIncrease.Should().BeLessOrEqualTo(lengthIncrease + 0.1);
            }
        }
    }
}