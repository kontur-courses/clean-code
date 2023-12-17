using Markdown.Contracts;

namespace Markdown.Tokens;

public class Token
{
    public Token(ITag? tag = null, string? text = null)
    {
        Text = text;
        Tag = tag;
    }

    public string? Text { get; }
    public ITag? Tag { get; }
}