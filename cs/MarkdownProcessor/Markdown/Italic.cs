namespace MarkdownProcessor.Markdown;

public class Italic : ITag
{
    public Italic(int startIndex)
    {
        StartIndex = startIndex;
    }

    public ITagMarkdownConfig Config { get; } = new ItalicConfig();
    public int StartIndex { get; }
    public int EndIndex { get; set; } = 0;
    public IEnumerable<ITag> Children { get; } = new List<ITag>();
    public bool Closed { get; } = false;

    public Token? RunTokenDownOfTree(Token token)
    {
        throw new NotImplementedException();
    }

    public ITag? RunTagDownOfTree(ITag tag)
    {
        throw new NotImplementedException();
    }
}