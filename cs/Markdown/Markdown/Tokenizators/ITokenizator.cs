namespace Markdown;

public interface ITokenizator
{
    public Tag Tag { get; }
    public string OpenTag { get; }
    public string CloseTag { get; }

    public List<TagToken> Tokenize(string mdstring);
}