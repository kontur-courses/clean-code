using System;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new Markdown().Render("__s _dsf_ f__"));
        }
    }
}
