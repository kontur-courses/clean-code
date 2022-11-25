namespace Markdown;

public class TagConfiguration
{
    public string Symbol;
    public string OpenTag;
    public string CloseTag;
    public int ClosePriority;

    public TagConfiguration(string symbol, string openTag, string closeTag, int closePriority)
    {
        Symbol = symbol;
        OpenTag = openTag;
        CloseTag = closeTag;
        ClosePriority = closePriority;
    }
}