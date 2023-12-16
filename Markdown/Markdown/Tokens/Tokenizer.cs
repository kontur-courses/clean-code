namespace Markdown;

public class Tokenizer
{
    private List<ITag> tags;

    public Tokenizer(List<ITag> tags)
    {
        this.tags = tags;
    }

    public IEnumerable<Token> Tokenize(string text)
    {
        throw new NotImplementedException();
    }
}