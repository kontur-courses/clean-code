namespace Markdown.Tokens;

internal class EOFToken : IToken
{
    public string Value => string.Empty;
    public int Length => 0;
}