using Markdown.Core;

namespace Markdown
{
    public static class MarkdownEngine
    {
        public static string Render(string mdText) => Tokenizer.ParseIntoTokens(mdText).ConvertToHtmlString();
    }
}