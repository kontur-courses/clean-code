using System;

namespace Markdown
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var input = "jj";
            Console.WriteLine(input);
            Console.WriteLine(new Md().Renderer(input));
        }
    }
}