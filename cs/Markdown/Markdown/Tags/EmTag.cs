namespace Markdown;

public class EmTag : Tag
{
    public string OpenHTMLTag => HTMLEmTag.OpenHTMLTag;
    public string CloseHTMLTag => HTMLEmTag.CloseHTMLTag;
    public string OpenMdTag => MarkdownEmTag.OpenTag;
    public string CloseMdTag => MarkdownEmTag.CloseTag;
}