using System;
using System.Collections.Generic;
using Markdown.Automaton;

namespace Markdown
{
    class Program
    {
        public static void Main()
        {
            var md = new Markdown();
            var html = md.Render("Привет_меня_зовут_Никита_");
            Console.WriteLine(html);
        }
    }
}
