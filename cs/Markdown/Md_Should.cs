using System;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_Should
    {
        [TestCase("# __d__ _dd_", "<h1> <strong>d</strong> <em>dd</em></h1>")]
        [TestCase("__d__", "<strong>d</strong>")]
        [TestCase("_d_", "<em>d</em>")]
        [TestCase("__d  _fa_ f__", "<strong>d  <em>fa</em> f</strong>")]
        [TestCase("_d  __fa__ f_", "<em>d  __fa__ f</em>")]
        [TestCase("_aa_aa", "<em>aa</em>aa")]
        [TestCase("aa_aa_", "aa<em>aa</em>")]
        [TestCase("aa_a_a", "aa<em>a</em>a")]
        [TestCase("a_aa _aa", "a_aa _aa")]
        [TestCase("a_3_aa aa", "a_3_aa aa")]
        [TestCase("a_1_", "a_1_")]
        [TestCase("a_ 1_", "a_ 1_")]
        [TestCase("_1_", "<em>1</em>")]
        [TestCase(@"\a_ 1_", @"\a_ 1_")]
        [TestCase(@"\a \_a_", @"\a _a_")]
        [TestCase(@"\a \\_a_", @"\a \<em>a</em>")]
        [TestCase("#_a \n\n a_\n\na", "<h1>_a </h1>\n\n a_\n\na")]
        [TestCase("_a \n\n\n a_\n\na", "_a \n\n\n a_\n\na")]
        [TestCase("[l](https://www.google.com \"F\")", "<a href=\"https://www.google.com\" title=\"F\">l</a>")]
        [TestCase("[l](https://www.google.com)", "<a href=\"https://www.google.com\">l</a>")]
        [TestCase("\\[l](https://www.google.com)", "[l](https://www.google.com)")]
        public void Render_Should(string input, string exeptedOutput)
        {
            Md.Render(input).Should().Be(exeptedOutput);
        }

        [TestCase(" __sf__ ", 100, 6)]
        [TestCase(" __sf__ ", 100, 2)]
        [TestCase(" sf ", 100, 8)]
        [TestCase(" _s_ \n\n __s__  ", 100, 8)]
        public void RenderTime_Should(string text, int repetitionsCount, int factor)
        {
            var firstInputBuilder = new StringBuilder();
            for (var i = 0; i < repetitionsCount; i++)
                firstInputBuilder.Append(text);

            var secondInputBuilder = new StringBuilder();
            for (var i = 0; i < repetitionsCount * factor; i++)
                secondInputBuilder.Append(text);

            var firstInput = firstInputBuilder.ToString();
            var secondInput = secondInputBuilder.ToString();

            Md.Render("");

            var firstTime = new TimeSpan();
            var secondTime = new TimeSpan();

            for (var j = 0; j < 1000; j++)
            {
                var firstTimer = new Stopwatch();
                firstTimer.Start();
                Md.Render(firstInput);
                firstTimer.Stop();

                firstTime += firstTimer.Elapsed;

                var secondTimer = new Stopwatch();
                secondTimer.Start();
                Md.Render(secondInput);
                secondTimer.Stop();
                secondTime += secondTimer.Elapsed;
            }

            secondTime.Should().BeLessThan(firstTime * factor);
        }
    }
}