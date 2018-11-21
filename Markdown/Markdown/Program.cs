using System;

namespace Markdown
{
    public class Program
    {
        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("Type some md text:");
                var mdInput = Console.ReadLine();
                var md = new Md();
                var result = md.Render(mdInput);
                Console.WriteLine("Result:");
                Console.WriteLine(result);
                Console.WriteLine();
            }
        }
    }
}