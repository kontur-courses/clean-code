using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MdRenderPerfomanceTest
    {
        private const string Pattern = "_asdf_ __asdf__ \n #asdf \n one";
        private const int RepeatSmallString = 10000;
        private const int RepeatsBigString = 1000000;
        
        [Test]
        public void RenderPerformance()
        {
            var md = new Md();
            var smallString = string.Concat(Enumerable.Repeat(Pattern, RepeatSmallString));
            var bigString = string.Concat(Enumerable.Repeat(Pattern, RepeatsBigString));
            var sw = Stopwatch.StartNew();
            
            md.Render(smallString);

            var renderSmallStringTime = sw.Elapsed;
            sw.Restart();
            
            md.Render(bigString);
            
            var renderBigString = sw.Elapsed;
            (renderBigString.Ticks / renderSmallStringTime.Ticks).Should()
                .BeLessOrEqualTo(RepeatsBigString / RepeatSmallString);
        }
    }
}