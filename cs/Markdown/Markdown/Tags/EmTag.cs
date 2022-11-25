namespace Markdown;

public class EmTag : Tag
{
    public string OpenHTMLTag => HTMLEmTag.OpenHTMLTag;
    public string CloseHTMLTag => HTMLEmTag.CloseHTMLTag;
    public string OpenMdTag => MarkdownEmTag.OpenMdTag;
    public string CloseMdTag => MarkdownEmTag.CloseMdTag;
}