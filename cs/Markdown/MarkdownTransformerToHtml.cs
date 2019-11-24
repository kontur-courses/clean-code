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
            new BlockquoteTagInfo(), 
        };
        
        internal static string Render(string inputString)
        {
            var tagParser = new TagParser(MarkdownTagsInfo);

            var tagTokens = tagParser.Parse(inputString)
                .RemoveEscapedTagTokens(inputString)
                .RemoveIncorrectTagSequences()
                .RemoveIncorrectNestingTagTokens();

            var outputString = HtmlConverter
                .ConvertToHtml(inputString, tagTokens)
                .RemoveEscapeSymbols();

            return outputString;
        }
    }
}
