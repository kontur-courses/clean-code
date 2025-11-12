namespace Markdown.Data;

public class Token
{
    private Tag _tag;
    private string _mark => Marks.GetMarkByTagName(_tag.Name);
    public string Head => _tag.OpenTag;
    public string Tail => _tag.CloseTag;
    public int StartPosition { get; }
    public int EndPosition { get; }

    public int Gap(bool isOpened)
    {
        if (_mark == Marks.Header || _mark == Marks.List)
        {
            if (isOpened)
            {
                return _mark.Length + 1;
            }
            return 0;
        }
        return _mark.Length;
    }

    public Token(Tag tag, int start, int end)
    {
        this._tag = tag;
        StartPosition = start;
        EndPosition = end;
    }

    public Token(string mark, int start, int end) : this(TagFactory.BuildTag(mark), start, end)
    {
    }
}