namespace MarkdownProcessor.Markdown;

public class FirstHeaderConfig : ITagMarkdownConfig
{
    public string OpeningSign => "# ";
    public string ClosingSign => "\n";
    public TextType TextType => TextType.FirstHeader;

    public ITag? CreateOrNull(Token token)
    {
        return !token.Before.HasValue ? new FirstHeader(token) : null;
    }
}