using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Program
    {
        static void Main()
        {
            var cases = new [] {"__a__", "b_"};
            var testing = new MarkdownTokenizer();
            
            var i = 1;
            foreach (var str in cases)
            {
                Console.WriteLine($"Case {i}: ");
                Console.WriteLine($"Current = \"{str}\", Length = {str.Length}");
                foreach (var token in testing.SplitTextToTokens(str))
                {
                    Console.WriteLine(token);
                }
                Console.WriteLine();
                i++;
            }
        }
    }


}