using System;

namespace Markdown
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var lexer = new Lexer();
            var tokenParser = new TokenParser();
            var htmlTokenRenderer = new HtmlTokenRenderer();
            var md = new Md(lexer, tokenParser, htmlTokenRenderer);
            Console.WriteLine(md.Render("__Выделенный двумя символами текст__"));
        }
    }
}