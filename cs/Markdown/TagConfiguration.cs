namespace Markdown;

public class TagConfiguration
{
    public readonly string Symbol;
    public readonly string OpenTag;
    public readonly string CloseTag;

    public TagConfiguration(string symbol, string openTag, string closeTag)
    {
        Symbol = symbol;
        OpenTag = openTag;
        CloseTag = closeTag;
    }
}