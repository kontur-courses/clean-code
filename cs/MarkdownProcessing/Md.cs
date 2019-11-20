using System;
using MarkdownProcessing.Converters;
using MarkdownProcessing.Markdowns;

namespace MarkdownProcessing
{
    public class Md
    {
        public string Render(string input, IResultMarkdown markdown)
        {
            return new MarkdownToTokenConverter(input, markdown).ParseInputIntoTokens();
        }
    }
}