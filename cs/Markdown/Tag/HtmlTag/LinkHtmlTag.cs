namespace Markdown;

public class LinkHtmlTag() : HtmlTag(true, "<a", "</a>")
{
    public static string StartLinkTag = "<a href=\"";
    //public static string EndLinkTag = ">";
    public static string EndLinkTag = "\"";
    public static string StartTitleTag = "title=\"";
    public static string EndTitleTag = "\"";
    public static string EndTag = "</a>";
}