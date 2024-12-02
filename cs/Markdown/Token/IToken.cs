namespace Markdown.Token;

public interface IToken
{
    public string SourceText { get; }
    public string ConvertedText { get; }
    public int Position { get; set; }
}