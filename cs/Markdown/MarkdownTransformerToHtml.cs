using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Markdown.MarkdownTags;

namespace Markdown
{
    internal static class MarkdownTransformerToHtml
    {
        private static readonly MarkdownTagInfo[] MarkdownTags = new MarkdownTagInfo[]
        {
            new CodeTagInfo(),
            new EmphasisTagInfo(),
            new StrongTagInfo(),
        };
        
        internal static string Render(string inputString)
        {
            var tagParser = new TagParser(MarkdownTags);

            var correctTags = tagParser.Parse(inputString)
                .RemoveEscapedTags(inputString)
                .RemoveUnopenedTags()
                .RemoveIncorrectNestingTags()
                .ToList();
            
            var outputString = HtmlConverter
                .ConvertToHtml(inputString, correctTags)
                .RemoveEscapeSymbols();

            return outputString;
        }
    }
}
