using Markdown.Tags;

namespace Markdown.Tokens;

public class StringToken(string str) : IToken
{
    public string Str { get; } = str;
    public List<IToken> Inner { get; } = new();
    public ITag Tag { get; } = new StringTag();
    
    public string MdString()
    {
        return Str;
    }
}