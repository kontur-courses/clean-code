namespace Markdown
{
    internal static class MarkdownTagToHTMLConverter
    {
        public static string GetTagByMarkdownAction(MarkdownActionType action, char c)
        {
            if (action == MarkdownActionType.Open) return OpenBrackets[c];
            else return CloseBrackets[c];
        }

        private static readonly Dictionary<char, string> OpenBrackets = new Dictionary<char, string>()
        {
            {'_', "<em>"},
            {';', "<strong>"},
            {'#', "<h1>"}
        };

        private static readonly Dictionary<char, string> CloseBrackets = new Dictionary<char, string>()
        {
            {'_', @"<\em>"},
            {';', @"<\strong>"},
            {'#', @"<\h1>"}
        };
    }
}