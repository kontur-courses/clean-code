using System;

namespace Markdown.Sandbox
{
    public class Program
    {
        public static void Main()
        {
            var markdown = new Core.Md();
            Console.WriteLine(markdown.Render("*qweqwe*"));
            Console.WriteLine(markdown.Render("_123_"));
        }
    }
}