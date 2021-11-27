using System;
using Markdown.Converters;

namespace Markdown
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(Md.Render("*+_a_+*"));
        }
    }
}