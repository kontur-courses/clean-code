using System;
using System.Text;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = "#abcd".SplitKeepSeparators(new[] { '#' });
            var a = new StringBuilder("abc");
            foreach (var item in "\\\\#abcd".SplitKeepSeparators(new[] { '_', '#', '\\' }))
            {

                Console.WriteLine(char.IsPunctuation('#'));
            }
        }
    }
}
