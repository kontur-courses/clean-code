namespace Markdown;

public class MdTagToken : IToken
{
    public ITag Tag { get; }
    public Status Status { get; private set; }
    private NeighboursContext Context { get; }
    public string GetValue => GetValueByStatus();

    public MdTagToken(ITag tag, NeighboursContext context)
    {
        Tag = tag;
        Context = context;
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
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}