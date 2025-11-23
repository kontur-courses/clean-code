namespace Markdown;

public interface ITokenGenerator
{
    List<Token> Tokenize(string text);
}