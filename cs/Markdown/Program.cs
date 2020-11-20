using System;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var md = new Md();
            var f = md.Render("__hello _world_!__");
            Console.WriteLine(f);
        }
    }
}
