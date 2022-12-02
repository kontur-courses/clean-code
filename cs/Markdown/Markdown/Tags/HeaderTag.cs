namespace Markdown;

public class HeaderTag : Tag
{
    public string OpenHTMLTag => HTMLHeaderTag.OpenHTMLTag;
    public string CloseHTMLTag => HTMLHeaderTag.CloseHTMLTag;
    public string OpenMdTag => MarkdownHeaderTag.OpenTag;
    public string CloseMdTag => MarkdownHeaderTag.CloseTag;
}