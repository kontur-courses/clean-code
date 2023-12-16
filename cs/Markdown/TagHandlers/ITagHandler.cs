namespace Markdown.TagHandlers;

public interface ITagHandler
{
    public string MdTag { get; }
    
    public string HtmlTag { get; }

    public string Render(string text, int startIndex = 0);

    public bool StartsWithTag(string text, int startIndex = 0);

    public bool IsValid(string text, int startIndex = 0);
    
    public int FindEndTagProcessing(string text, int startIndex = 0);
}