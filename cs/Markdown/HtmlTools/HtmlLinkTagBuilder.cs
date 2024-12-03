namespace Markdown.HtmlTools;

internal static class HtmlLinkTagBuilder
{
    public static string Build(string linkMarkdown)
    {
        var indexDividesLabelAndLink = linkMarkdown.IndexOf(']');

        if (indexDividesLabelAndLink <= 0)
        {
            return "";
        }

        var indexWhereLinkStarts = indexDividesLabelAndLink + 2;
        var link = linkMarkdown.Substring(indexWhereLinkStarts, linkMarkdown.Length - indexWhereLinkStarts - 1);
        var label = linkMarkdown[1..indexDividesLabelAndLink];

        return $"<a href={link}>{label}</a>";
    }
}