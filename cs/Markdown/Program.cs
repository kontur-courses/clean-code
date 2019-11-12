using System;

namespace Markdown
{
    public class Program
    {
        static void Main(string[] args)
        {
            var markdownRenderer = new MdRenderer();
            Console.WriteLine(markdownRenderer.Render("text _em em_ __strong strong__"));
        }
    }
}