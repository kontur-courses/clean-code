namespace Markdown;

public record class ParseResult(bool HasToken, Token? Token, int TokenLength)
{
    public static ParseResult NullResult => new(false, null, -1);
}
