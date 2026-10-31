namespace Markdown.Tokens;

public record Token(TokenKind Kind, string Value, bool IsClosable, bool IsOpenable);

public enum TokenKind
{
    Text,
    Em,
    Strong
}