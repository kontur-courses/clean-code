namespace Markdown;

public class Token
{
    public int Position { get; set; }
    public string? Text { get; set; }
    public Tag? Tag { get; set; }

    public Token(int position, Tag? tag = null, string? text = null)
    {
        Position = position;
        Text = text;
        Tag = tag;
    }
}