namespace Markdown.Tags;

public abstract class BlockTag : ITag
{
    public abstract string MdTag { get; }
    public abstract string HtmlOpenTag { get; }
    public abstract string HtmlCloseTag { get; }
    
    public abstract string HtmlBlockOpenTag { get; }
    
    public abstract string HtmlBlockCloseTag { get; }
    
    public bool CanBeOpened(char left, char right)
    {
        return left is '\0' or '\n';
    }

    public bool CanBeClosed(char left, char right)
    {
        return true;
    }
}