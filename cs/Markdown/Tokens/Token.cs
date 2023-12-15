using Markdown.Contracts;

namespace Markdown.Tokens;

public class Token
{
    public Token(int position, ITag? tag = null, string? text = null)
    {
        Position = position;
        Text = text;
        Tag = tag;
    }

    public int Position { get; }
    public string? Text { get; }
    public ITag? Tag { get; }
}