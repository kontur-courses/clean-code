namespace MarkdownProcessor.Markdown;

public abstract class HighlightTextPairTag : ITag
{
    private bool corrupted;
    private bool processedWhiteSpace;

    protected HighlightTextPairTag(Token openingToken)
    {
        OpeningToken = openingToken;
    }

    public abstract ITagMarkdownConfig Config { get; }
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

        if (IsClosingToken(token))
        {
            if (Children.All(t => t.Closed))
            {
                ClosingToken = token;
                Closed = true;
                return null;
            }

            corrupted = true;
        }

        return token;
    }

    public void RunTagDownOfTree(ITag tag)
    {
        if (corrupted) return;

        if (Closed) throw new InvalidOperationException();

        if (Children.Any() && !Children.Last().Closed)
            Children.Last().RunTagDownOfTree(tag);
        else
            Children.Add(tag);
    }

    private bool IsClosingToken(Token token)
    {
        return token.Value == Config.ClosingSign &&
               !token.BeforeIsSpace &&
               !token.BetweenDigits &&
               !BlankStringBetweenToken(OpeningToken, token) &&
               !(processedWhiteSpace && HighlightWordPart(OpeningToken, token));
    }

    private static bool BlankStringBetweenToken(Token opening, Token closing)
    {
        return opening.TagFirstCharIndex + opening.Value.Length == closing.TagFirstCharIndex;
    }

    private static bool HighlightWordPart(Token opening, Token closing)
    {
        return !string.IsNullOrWhiteSpace(opening.Before.ToString()) ||
               !string.IsNullOrWhiteSpace(closing.After.ToString());
    }
}