namespace Markdown.TokenBuilder;

public interface ITokenBuilder
{
    List<Token.Token> BuildTokens(string text);
}