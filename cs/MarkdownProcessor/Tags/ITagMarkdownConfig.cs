namespace MarkdownProcessor.Tags;

public interface ITagMarkdownConfig
{
    public string OpeningSign { get; }

    public string ClosingSign { get; }

    public TextType TextType { get; }

    public Tag? CreateOrNull(Token token);
}