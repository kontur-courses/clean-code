namespace Markdown;

public class StrongTag : Tag
{
    public string OpenHTMLTag => "<strong>";
    public string CloseHTMLTag => "</strong>";
    public string OpenMdTag => "__";
    public string CloseMdTag => "__";
}