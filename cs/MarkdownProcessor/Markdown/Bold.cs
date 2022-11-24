namespace MarkdownProcessor.Markdown;

public class Bold : ITag
{
    private readonly bool openInsideWord;
    private bool corrupted;
    private bool processedWhiteSpace;

    public Bold(Token openingToken, bool openInsideWord)
    {
        OpeningToken = openingToken;
        this.openInsideWord = openInsideWord;
    }

    public ITagMarkdownConfig Config { get; } = new BoldConfig();
    public Token OpeningToken { get; }
    public Token ClosingToken { get; private set; }
    public List<ITag> Children { get; } = new();
    public bool Closed { get; private set; }

    public Token? RunTokenDownOfTree(Token token)
    {
        if (Closed) throw new InvalidOperationException();

        if (corrupted) return token;

        if (string.IsNullOrWhiteSpace(token.Value)) processedWhiteSpace = true;

        if (Children.Any() && !Children.Last().Closed)
        {
            var nullableToken = Children.Last().RunTokenDownOfTree(token);
            if (nullableToken is null) return nullableToken;
            token = nullableToken.Value;
        }

        if (Config.OpeningSign != token.Value || !IsClosingToken(token)) return token;

        if (Children.Any(t => !t.Closed))
        {
            corrupted = true;
            return token;
        }

        ClosingToken = token;
        Closed = true;
        return null;
    }

    public void RunTagDownOfTree(ITag tag)
    {
        if (Closed) throw new InvalidOperationException();

        if (corrupted) return;
        if (Children.Any() && !Children.Last().Closed)
            Children.Last().RunTagDownOfTree(tag);
        else
            Children.Add(tag);
    }

    private bool IsClosingToken(Token token)
    {
        var beforeIsSpace = string.IsNullOrWhiteSpace(token.Before.ToString());
        var spaceAndWordPart =
            processedWhiteSpace && (openInsideWord || !string.IsNullOrWhiteSpace(token.After.ToString()));
        return !spaceAndWordPart && !beforeIsSpace && !token.BetweenDigits &&
               OpeningToken.TagFirstCharIndex + OpeningToken.Value.Length < token.TagFirstCharIndex;
    }
}