using System;
using System.Collections.Generic;

namespace Markdown
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var text = Console.ReadLine();
            Console.WriteLine(Translator.Translate(text));
        }
    }
}