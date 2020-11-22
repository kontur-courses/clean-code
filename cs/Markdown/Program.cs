using System;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Md.Render("# Заголовок __с _разными_ символами__\n"));
        }
    }
}