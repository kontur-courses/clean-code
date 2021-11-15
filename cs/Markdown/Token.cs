namespace Markdown;

internal class Token
{
    private readonly string source;
    private readonly int start;
    private readonly int length;

    public Token(string source, int start, int length)
    {
        this.source = source;
        this.start = start;
        this.length = length;
    }

    public string ExtractToken()
    {
        return source.Substring(start, length);
    }
}
