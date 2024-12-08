namespace Markdown.interfaces;

public interface IMarkdownConverter
{
    string Convert(IEnumerable<Token.Token> tokens);
}