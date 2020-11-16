using System;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    public class MdRenderPerfomanceTest
    {
        [Test]
        public void RenderPerformance()
        {
            var pattern = "_asdf_ __asdf__ \n #asdf \n one";
            var md = new Md();
            var repeatsLittleString = 10000;
            var repeatsBigString = 1000000;
            var smallString = string.Concat(Enumerable.Repeat(pattern, repeatsLittleString));
            var bigString = string.Concat(Enumerable.Repeat(pattern, repeatsBigString));
            var sw = Stopwatch.StartNew();
            
            md.Render(smallString);

            var renderSmallStringTime = sw.Elapsed;
            Console.WriteLine(renderSmallStringTime);
            sw.Restart();
            
            md.Render(bigString);
            
            var renderBigString = sw.Elapsed;
            Console.WriteLine(renderBigString);
            Console.WriteLine(repeatsBigString / repeatsLittleString);
            var coefficient = renderBigString.Ticks / renderSmallStringTime.Ticks;
            coefficient.Should().BeLessOrEqualTo(repeatsBigString / repeatsLittleString);
        }
    }
}