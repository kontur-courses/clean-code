namespace Markdown;

public class TokenWithArgument(TokenType type, string text, string argument) : Token(type, text, null)
{
    public readonly string Argument = argument;
}
