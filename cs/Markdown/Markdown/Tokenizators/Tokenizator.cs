namespace Markdown;

public interface Tokenizator
{
    //TODO: имплементировать в другое место
    public string OpenTag { get; }
    public string CloseTag { get; }
    public Tag Tag { get; }
    public List<TagToken> Tokenize(string mdstring);
}