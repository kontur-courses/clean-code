namespace Markdown;

public class EmTag : Tag
{
    public string OpenHTMLTag => "<em>";
    public string CloseHTMLTag => "</em>";
    public string OpenMdTag => "_";
    public string CloseMdTag => "_";
}