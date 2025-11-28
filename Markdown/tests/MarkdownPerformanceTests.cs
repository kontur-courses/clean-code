using System;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Markdown;

namespace Markdown.tests
{
    [TestFixture]
    public class MarkdownPerformanceTests
    {
        private Md md;

        [SetUp]
        public void Setup()
        {
            md = new Md();
        }

        private string MakeInput(int minLength)
        {
            var sb = new StringBuilder(minLength + 100);
            int i = 0;
            while (sb.Length < minLength)
            {
                sb.Append("слово");
                sb.Append(i % 10);
                sb.Append("_");
                sb.Append("внутри");
                sb.Append(i % 100);
                sb.Append("__bold__ ");
                if (i % 7 == 0) sb.Append(@"\_esc\_ ");
                i++;
            }
            return sb.ToString();
        }

        [Test]
        public void Render_ShouldScaleApproximatelyLinearly()
        {

            int n1 = 50_000;
            int n2 = 2_000_000;
            var s1 = MakeInput(n1);
            var s2 = MakeInput(n2);

            var sw = Stopwatch.StartNew();
            md.Render(s1);
            sw.Stop();
            double t1 = sw.Elapsed.TotalMilliseconds;

            sw.Restart();
            md.Render(s2);
            sw.Stop();
            double t2 = sw.Elapsed.TotalMilliseconds;

            double perChar1 = t1 / s1.Length;
            double perChar2 = t2 / s2.Length;

            TestContext.WriteLine($"Размер1={s1.Length}, время1={t1} ms, на символ={perChar1} ms");
            TestContext.WriteLine($"Размер2={s2.Length}, время2={t2} ms, на символ={perChar2} ms");

            // Допустимый коэффициент роста времени на символ.
            double allowedFactor = 3.0;

            perChar2.Should().BeLessThan(perChar1 * allowedFactor);
        }
    }
}
