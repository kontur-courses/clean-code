namespace Markdown;

public interface ITokenRule
{
    Token? TryReadTokenAndMoveCursor(TextCursor cursor);
}