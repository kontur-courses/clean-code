namespace Markdown;

public class StrongTag : Tag
{
    public string OpenHTMLTag => HTMLStrongTag.OpenHTMLTag;
    public string CloseHTMLTag => HTMLStrongTag.CloseHTMLTag;
    public string OpenMdTag => MarkdownStrongTag.OpenMdTag;
    public string CloseMdTag => MarkdownStrongTag.CloseMdTag;
}