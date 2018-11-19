using System;
using System.IO;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Console.ReadLine();
            if (path == null) return;
            var content = File.ReadAllText(path);
            var md = new Md();
            Console.WriteLine(md.Render(content));
        }
    }
}
