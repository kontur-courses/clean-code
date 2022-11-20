namespace Markdown;

public interface Tag
{
    public string OpenHTMLTag { get; }
    public string CloseHTMLTag { get; }
    public string OpenMdTag { get; }
    public string CloseMdTag { get; }
}