namespace Markdown.Token;

public class Token(string sourceText, string convertedText, int position) : IToken
{
    public string SourceText { get; } = sourceText;
    public string ConvertedText { get; } = convertedText;
    public int Position { get; set; } = position;
}