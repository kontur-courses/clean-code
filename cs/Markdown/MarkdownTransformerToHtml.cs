using Markdown.MarkdownTags;

namespace Markdown
{
    internal static class MarkdownTransformerToHtml
    {
        private static readonly MarkdownTagInfo[] MarkdownTagsInfo = new MarkdownTagInfo[]
        {
            new CodeTagInfo(),
            new EmphasisTagInfo(),
            new StrongTagInfo(),
        };
        
        internal static string Render(string inputString)
        {
            var tagParser = new TagParser(MarkdownTagsInfo);

            var correctTags = tagParser.Parse(inputString)
                .RemoveEscapedTags(inputString)
                .RemoveUnpairedTags()
                .RemoveIncorrectNestingTags();

            var outputString = HtmlConverter
                .ConvertToHtml(inputString, correctTags)
                .RemoveEscapeSymbols();

            return outputString;
        }
    }
}
