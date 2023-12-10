namespace Markdown;

public class Wrapper : IWrapper
{
    private Tags tags;

    public Wrapper(Tags tags)
    {
        this.tags = tags;
    }

    public IEnumerable<string> WrapTokensInTags(IEnumerable<Token> tokens)
    {
        throw new NotImplementedException();
    }
}