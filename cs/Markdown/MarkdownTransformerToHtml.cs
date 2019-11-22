using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Markdown
{
    internal static class MarkdownTransformerToHtml
    {
        private static readonly MarkdownTag[] MarkdownTags = new MarkdownTag[]
        {
            new CodeTag(),
            new EmphasisTag(),
            new StrongTag(),
        };
        
        internal static string Render(string inputString)
        {
            var tagParser = new TagParser(MarkdownTags);

            var correctTags = tagParser.Parse(inputString)
                .RemoveEscapedTags(inputString)
                .RemoveUnopenedTags()
                .RemoveIncorrectNestingTags()
                .ToList();

            var outputString = inputString;
            outputString = StringRedactor.SwitchMarkdownTagsToHtml(outputString, correctTags);
            outputString = StringRedactor.RemoveEscapeSymbols(outputString);

            return outputString;
        }
    }
}
