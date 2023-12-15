namespace Markdown.TagHandlers;

public interface ITagHandler
{
    public string Render(string s, int startIndex = 0);
    
    public string Tag { get; }

    public bool StartsWithTag(string s, int startIndex = 0);

    public bool IsValid(string s, int startIndex = 0);
    
    public int FindEndTagProcessing(string s, int startIndex = 0);
}