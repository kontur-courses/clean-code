namespace Markdown.Data;

public class PositionedTag : Tag
{
    public int Position { get; }
    public bool IsOpenClose { get; }
    public bool IsOpening { get; }
    public string Mark => Marks.GetMarkByTagName(Name);

    public PositionedTag(int position, string mark, bool isOpening = true, bool isOpenClose = false) : base(TagFactory.BuildTag(mark).Name)
    {
        IsOpenClose = isOpenClose;
        IsOpening = isOpening;
        Position = position;
    }
}