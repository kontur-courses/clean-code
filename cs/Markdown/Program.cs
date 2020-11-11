using System;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = "#abcd".SplitKeppSeparators(new[] { '#' });
            foreach (var item in "__asd__sdf_a_____".SplitKeppSeparators(new[] { '_'}))
            {
                Console.WriteLine(item);
            }
        }
    }
}
