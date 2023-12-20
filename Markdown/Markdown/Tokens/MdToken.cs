namespace Markdown;

public class MdToken : IToken<ITag>
{
    public ITag? Tag { get; }
    public string? Text { get; }

    public MdToken(string text)
    {
        Text = text;
    }

    public MdToken(ITag tag)
    {
        Tag = tag;
    }
}