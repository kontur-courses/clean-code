using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTask
{
    internal class Markdown
    {
        private readonly ITagParser[] Parsers;

        public Markdown(ITagParser[] parsers)
        {
            Parsers = parsers;
        }

        public string Render(string markdownText)
        {
            var tokens = new List<Token>();

            foreach (var parser in Parsers)
            {
                tokens.AddRange(parser.Parse(markdownText));
            };


            return HtmlProcessor.Process(markdownText, tokens); ;
        }
    }
}