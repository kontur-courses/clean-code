using System;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Md.Render("_text_"));
            Console.WriteLine(Md.Render("#text\n _text_"));
        }
    }
}