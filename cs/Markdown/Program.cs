using System;

namespace Markdown
{
    class Program
    {
        public static void Main()
        {
            var text = "_F_";
            var result = MarkdownСomponents.Markdown.Render(text);
            Console.WriteLine($"\nResult: {result}");
        }
    }
}
