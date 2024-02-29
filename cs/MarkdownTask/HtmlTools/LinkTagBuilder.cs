namespace MarkdownTask
{
    public class LinkTagBuilder
    {
        public static string Build(string linkMarkdown)
        {
            var split = linkMarkdown.IndexOf("]");

            if (split <= 0)
                return "";

            var label = linkMarkdown[1..split];
            var link = linkMarkdown.Substring(split + 2, linkMarkdown.Length - (split + 2) - 1);

            return $"<a href={label}>{link}</a>";
        }
    }
}
