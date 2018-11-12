using System;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = Md.Render("__Hello_World__");
            Console.WriteLine(str);
        }
    }
}