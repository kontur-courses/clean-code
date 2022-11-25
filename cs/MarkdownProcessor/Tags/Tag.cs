namespace MarkdownProcessor.Tags;

public abstract class Tag
{
    protected Tag(Token openingToken)
    {
        OpeningToken = openingToken;
    }

    public abstract ITagMarkdownConfig Config { get; }
    public Token OpeningToken { get; }
    public Token ClosingToken { get; private set; }
    public List<Tag> Children { get; } = new();
    public bool Closed { get; private set; }

    public Token? RunTokenDownOfTree(Token token)
    {
        if (Closed) throw new InvalidOperationException();

        if (StopRunToken(token)) return token;

        if (Children.Any() && !Children.Last().Closed)
        {
            var nullableToken = Children.Last().RunTokenDownOfTree(token);
            if (nullableToken is null) return nullableToken;
            token = nullableToken.Value;
        }

        if (!IsClosingToken(token)) return token;

        ClosingToken = token;
        Closed = true;
        return null;
    }

    public void RunTagDownOfTree(Tag tag)
    {
        if (Closed) throw new InvalidOperationException();

        if (StopRunTag(tag)) return;

        if (Children.Any() && !Children.Last().Closed)
            Children.Last().RunTagDownOfTree(tag);
        else
            Children.Add(tag);
    }

    protected abstract bool StopRunToken(Token token);

    protected abstract bool StopRunTag(Tag tag);

    protected abstract bool IsClosingToken(Token token);
}