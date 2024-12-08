namespace Markdown;

public interface IMarkdownConverter
{
    public string Convert(List<Token> tokens);
}
