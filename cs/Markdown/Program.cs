using System;
using Markdown.Core;

namespace Markdown
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(MarkdownEngine.Render("# __title__\n__text__"));
            //Console.WriteLine(MarkdownEngine.Render("__some _meaningful_ text__ __text _without_ _so_me mean__"));
        }
    }
}