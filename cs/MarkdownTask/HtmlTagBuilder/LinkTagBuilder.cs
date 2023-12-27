namespace MarkdownTask
{
    public class LinkTagBuilder
    {
        public static string Build(string linkMarkdown)
        {
            var split = linkMarkdown.IndexOf("]");
            if (split <= 0)
                return "";

            return "<a href=" + linkMarkdown.Substring(1, split - 1) + ">" + linkMarkdown.Substring(split + 2) + "</a>";
        }
    }
}
