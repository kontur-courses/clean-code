namespace Markdown;

public class MdTagToken : IToken
{
    public ITag Tag { get; }
    public Status Status { get; private set; }
    public string Value => GetValueByStatus();
    public int Length => Tag.MdTag.Length;
    public (char, char) Context;

    public MdTagToken(ITag tag)
    {
        Tag = tag;
    }

    public void SetStatus(Status status)
    {
        Status = status;
    }

    private string GetValueByStatus()
    {
        return Status switch
        {
            Status.Broken => Tag.MdTag,
            Status.Opened => Tag.HtmlTag.Open,
            Status.Closed => Tag.HtmlTag.Close,
            _ => throw new InvalidOperationException("Status was unset before accessing")
        };
    }
}