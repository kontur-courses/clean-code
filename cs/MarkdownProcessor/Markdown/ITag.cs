namespace MarkdownProcessor.Markdown;

public interface ITag
{
    public ITagMarkdownConfig Config { get; }
    public int StartIndex { get; }
    public int EndIndex { get; set; }
    public IEnumerable<ITag> Children { get; }
    public bool Closed { get; }
    public Token? RunTokenDownOfTree(Token token);
    public ITag? RunTagDownOfTree(ITag tag);
}