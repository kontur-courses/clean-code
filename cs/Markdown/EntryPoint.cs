using System;
using Markdown.Languages;
using Markdown.Tokenizing;

namespace Markdown
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            var res = MarkdownTokenizer.Tokenize("_hello_ people");
        }
    }
}
