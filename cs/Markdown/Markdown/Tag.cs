namespace Markdown;

public interface Tag
{
    public string OpenHTMLTag { get; }
    public string CloseHTMLTag { get; }
    public string OpenMdTag { get; }
    public string CloseMdTag { get; }
}

public class EmptyTag : Tag
{
    public string OpenHTMLTag => "";
    public string CloseHTMLTag => "";
    public string OpenMdTag => "";
    public string CloseMdTag => "";
}

public class EmTag : Tag
{
    public string OpenHTMLTag => "<em>";
    public string CloseHTMLTag => "</em>";
    public string OpenMdTag => "_";
    public string CloseMdTag => "_";
}