namespace Markdown;

public class MarkdownBuilder
{
    private readonly string source;
    private readonly Tokenizer tokenizer;

    public MarkdownBuilder(string source, List<ITag> tags)
    {
        this.source = source;
        tokenizer = new Tokenizer(tags);
    }
    
    public string Build()
    {
        var tokens = tokenizer.Tokenize(source);
        throw new NotImplementedException();
    }
}