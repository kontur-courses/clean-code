using Markdown.Types;

namespace Markdown;

public class ParsedString
{
    public IType Type { private set; get; }
    public int Start { private set; get; }
    public int End { set; get; }
    public string Prefix { private set; get; }
    public ParsedString(IType type, int start, string prefix)
    {
        this.Start = start;
        this.Type = type;
        this.Prefix = prefix;
    }
}