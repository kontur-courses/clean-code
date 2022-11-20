namespace MarkdownProcessor.Markdown;

public interface ITagMarkdownConfig
{
    public string Sign { get; }
    public TextType TextType { get; }

    public ITag? TryCreate(Token token);

    public bool IsClosingToken(Token token);
}