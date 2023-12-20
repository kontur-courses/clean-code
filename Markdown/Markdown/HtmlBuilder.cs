namespace Markdown;

public class HtmlBuilder
{
    private readonly string source;
    private readonly MdTokenizer mdTokenizer;

    public HtmlBuilder(string source)
    {
        this.source = source;
        mdTokenizer = new MdTokenizer(this.source);
    }
    
    public string Build()
    {
        var tokens = mdTokenizer.Tokenize();
        throw new NotImplementedException();
    }
}