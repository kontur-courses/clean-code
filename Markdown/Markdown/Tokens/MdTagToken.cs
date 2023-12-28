namespace Markdown;

public class MdTagToken : IToken
{
    public ITag Tag { get; }
    public Status Status { get; set; }
    public string Value => GetValueByStatus();
    public int Length => Tag.MdTag.Length;
    public (char Left, char Right) AdjacentSymbols { get; private set; }
    public bool IsInsideWord => char.IsLetterOrDigit(AdjacentSymbols.Left) && char.IsLetterOrDigit(AdjacentSymbols.Right);

    public MdTagToken(ITag tag)
    {
        Tag = tag;
    }

    public void SetContext(char left, char right)
    {
        if (char.IsDigit(left) || char.IsDigit(right))
            Status = Status.Broken;
        AdjacentSymbols = (left, right);
    }

    private string GetValueByStatus()
    {
        return Status switch
        {
            Status.Broken => Tag.MdTag,
            Status.Opened => Tag.HtmlTag,
            Status.Closed => Tag.HtmlTag.Insert(1, "/"),
            _ => throw new InvalidOperationException("Status was unset before accessing")
        };
    }
}