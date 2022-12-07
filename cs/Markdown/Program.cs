using System;

namespace Markdown
{
    public class Program
    {
        public static void Main()
        {
            var markdownText = @"";
            var md = new Md();
            Console.WriteLine(md.Render(markdownText));
        }
    }
}
