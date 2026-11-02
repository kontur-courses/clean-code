namespace Markdown;

public class DelimiterParser
{
    private readonly EscapeParser escapeParser;

    public DelimiterParser(EscapeParser escapeParser)
    {
        this.escapeParser = escapeParser;
    }

    public IReadOnlyList<DelimiterPair> FindPairs(string text)
    {
        throw new NotImplementedException();
    }

    private bool CanOpen(string text, int delimiterStartIndex, int delimiterLength)
    {
        throw new NotImplementedException();
    }

    private bool CanClose(string text, int delimiterStartIndex, int delimiterLength)
    {
        throw new NotImplementedException();
    }
}
