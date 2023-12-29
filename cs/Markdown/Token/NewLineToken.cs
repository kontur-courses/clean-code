namespace Markdown.Token;

public class NewLineToken : IToken
{
    private const bool HasPair = false;
    private readonly string tokenSeparator = "\n";
    public int Position { get; }
    public int Length => tokenSeparator.Length;
    public string Separator => tokenSeparator;
    public bool IsPair => HasPair;
    public bool IsClosed { get; set; }
    public bool IsParametrized => false;
    public List<string> Parameters { get; set; }
    public int TokenSymbolsShift { get; set; }

    public NewLineToken(int position, string separator = "\n")
    {
        Position = position;
        tokenSeparator = separator;
    }

    public bool IsValid(string source, List<IToken> tokens, IToken currentToken)
    {
        return true;
    }
}