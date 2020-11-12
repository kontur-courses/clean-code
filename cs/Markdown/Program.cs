using System;
using System.Text;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = "#abcd".SplitKeppSeparators(new[] { '#' });
            var a = new StringBuilder("abc");
            foreach (var item in "__ab__cd__ef__".SplitKeppSeparators(new[] { '_'}))
            {
                Console.WriteLine(item);
            }
        }
    }
}
