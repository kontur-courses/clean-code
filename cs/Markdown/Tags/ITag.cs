namespace Markdown.Tags;

public interface ITag
{
    public string MdTag { get; }
    public string HtmlTag { get; }
    public bool IsOpenedCorrectly((char left, char right) adjacentChars);
    public bool IsClosedCorrectly((char left, char right) adjacentChars);
    
}