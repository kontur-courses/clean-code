using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Markdown
{
    public class MdRenderPerfomanceTest
    {
        [Test]
        public void UpcPerformance()
        {
            var pattern = "_asdf_ __asdf__ \n #asdf \n one";
            var md = new Md();
            var smallString = string.Concat(Enumerable.Repeat(pattern, 10000));
            var bigString = string.Concat(Enumerable.Repeat(pattern, 1000000));
            var sw = Stopwatch.StartNew();
            
            md.Render(smallString);

            Console.WriteLine("for x " + sw.Elapsed);
            sw.Restart();
            
            md.Render(bigString);
            
            Console.WriteLine("for 100 x " + sw.Elapsed);
        }
    }
}