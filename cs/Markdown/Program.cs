using System;
using Markdown.TokenRenderer;

namespace Markdown
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var lexer = new Lexer.Lexer();
            var tokenParser = new TokenParser.TokenParser();
            var htmlTokenRenderer = new HtmlTokenRenderer();
            var md = new Md(lexer, tokenParser, htmlTokenRenderer);
            Console.WriteLine(md.Render("__Выделенный двумя символами текст__"));
        }
    }
}