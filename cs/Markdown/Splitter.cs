namespace Markdown;

public class Splitter : ISplitter
{
    private Tags tags;

    public Splitter(Tags tags)
    {
        this.tags = tags;
    }

    public IEnumerable<Token> SplitToTokens(string markdownText)
    {
        throw new NotImplementedException();
    }
}