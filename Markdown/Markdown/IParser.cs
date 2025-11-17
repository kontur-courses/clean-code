namespace Markdown;

public interface IMarkdownParser
{
    public string Render(string markdown);

    public List<Token> TokenizeText(string markdown);

    public string BuildHTMLString(List<Token> tokens, string markdown);
}