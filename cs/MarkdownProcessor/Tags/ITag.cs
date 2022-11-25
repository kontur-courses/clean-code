namespace MarkdownProcessor.Tags;

public interface ITag
{
    public ITagMarkdownConfig Config { get; }
    public Token OpeningToken { get; }
    public Token ClosingToken { get; }
    public List<ITag> Children { get; }
    public bool Closed { get; }
    public Token? RunTokenDownOfTree(Token token);
    public void RunTagDownOfTree(ITag tag);
}