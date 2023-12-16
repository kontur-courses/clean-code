namespace Markdown.TextTokenizer;

public interface ITextTokenizer
{
    List<Token.Token> Split(string text);
}