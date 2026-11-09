namespace Markdown;

public interface ILexer
{
    IEnumerable<Token> GetTokens(string text);
}