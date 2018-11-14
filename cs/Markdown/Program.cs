using System;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var md = new Md();
            var renderedText = md.Render("__hello__ _world_");
            Console.WriteLine(renderedText);
        }
    }
}
