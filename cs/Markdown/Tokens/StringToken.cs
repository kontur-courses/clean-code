using Markdown.Tags;

namespace Markdown.Tokens;

public class StringToken(string str) : Token
{
    public override string Str { get; } = str;
    protected override Tag Tag { get; } = new StringTag();
    
    public override string MdString()
    {
        return Str;
    }
}