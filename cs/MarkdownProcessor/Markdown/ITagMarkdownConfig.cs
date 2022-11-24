namespace MarkdownProcessor.Markdown;

public interface ITagMarkdownConfig
{
    public string OpeningSign { get; }

    public string ClosingSign { get; }

    public TextType TextType { get; }

    public ITag? CreateOrNull(Token token);
}