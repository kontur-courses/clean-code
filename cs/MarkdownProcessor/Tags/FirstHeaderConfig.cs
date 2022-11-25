namespace MarkdownProcessor.Tags;

public class FirstHeaderConfig : ITagMarkdownConfig
{
    public string OpeningSign => "# ";
    public string ClosingSign => "\n";
    public TextType TextType => TextType.FirstHeader;

    public Tag? CreateOrNull(Token token)
    {
        return !token.Before.HasValue ? new FirstHeader(token) : null;
    }
}