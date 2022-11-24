namespace Markdown
{
    internal static class MarkdownToHTMLTagConverter
    {
        public static string GetTagByMarkdownAction(MarkdownActionType action)
        {
            return dict[action];
        }

        private static Dictionary<MarkdownActionType, string> dict = new Dictionary<MarkdownActionType, string>()
        {
            {MarkdownActionType.OpenCursive, "<em>"},
            {MarkdownActionType.CloseCursive, @"<\em>"}
        };
    }
}