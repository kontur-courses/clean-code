namespace Markdown.Token;

public class NewLineToken : IToken
{
    private const string TokenSeparator = "\n";
    private const bool HasPair = false;
    public int Position { get; set; }
    public int Length => TokenSeparator.Length;
    public string Separator => TokenSeparator;
    public bool IsPair => HasPair;
    public bool IsClosed { get; set; }
    public bool IsParametrized => false;
    public List<string> Parameters { get; set; }
    public int TokenSymbolsShift { get; set; }

    public NewLineToken(int position)
    {
        Position = position;
    }

    public bool IsValid(string source, List<IToken> tokens, IToken currentToken)
    {
        return true;
    }
}