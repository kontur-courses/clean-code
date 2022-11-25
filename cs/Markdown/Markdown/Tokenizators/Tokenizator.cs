namespace Markdown;

public interface Tokenizator
{
    public string OpenTag { get; }
    public string CloseTag { get; }
    public List<TagToken> Tokenize(string mdstring);
}