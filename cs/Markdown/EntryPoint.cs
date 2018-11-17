using System;
using Markdown.Languages;
using Markdown.Parsing;
using Markdown.Tokenizing;

namespace Markdown
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            var markSource = "hello __people I'm _so_ glad__ to see __you__";
            var tokens = new Tokenizer(new MarkdownLanguage()).Tokenize(markSource);
            Console.WriteLine(new Parser(new HtmlLanguage()).Parse(tokens));
        }
    }
}
