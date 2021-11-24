using System;
using Markdown.TagRenderer;

namespace Markdown
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var lexer = new Lexer.Lexer();
            var tokenParser = new TokenParser.TokenParser();
            var htmlTokenRenderer = new HtmlTagRenderer();
            var md = new MarkdownConverter(lexer, tokenParser, htmlTokenRenderer);
            Console.WriteLine(md.Render("__Выделенный двумя символами текст__"));
        }
    }
}