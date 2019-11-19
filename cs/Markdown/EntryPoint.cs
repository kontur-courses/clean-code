using System;

namespace Markdown
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            var md = new Md();
            while (true)
                Console.WriteLine(md.Render(Console.ReadLine()));
        }
    }
}