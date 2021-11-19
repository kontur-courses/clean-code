namespace Markdown;

internal class TokenBuilder
{
    public Tokenizer TokenWrapper { get; private set; }
    public string Source { get; private set; }
    public int Start { get; private set; }
    public int End { get; private set; }
    public string? MdTag { get; private set; }

    private readonly List<TokenBuilder> tokens = new();

    public TokenBuilder(Tokenizer tokenWrapper, string source)
    {
        TokenWrapper = tokenWrapper;
        Source = source;
    }

    public TokenBuilder WithStart(int start)
    {
        Start = start;
        return this;
    }

    public TokenBuilder WithEnd(int end)
    {
        End = end;
        return this;
    }

    public TokenBuilder WithMdTag(string? mdTag)
    {
        MdTag = mdTag;
        return this;
    }

    public void AddToken(TokenBuilder token)
    {
        tokens.Add(token);
    }

    internal Token Build()
    {
        if (tokens.Count == 0)
            return TokenWrapper.WrapToken(Source[Start..End], Start, MdTag);
        return BuildCompound();
    }

    private CompoundToken BuildCompound()
    {
        var setting = TokenWrapper.GetSetting(MdTag);
        var compound = new CompoundToken(Source[Start..End], Start, setting);
        foreach (var token in tokens)
        {
            compound.AddToken(token.Build());
        }

        return compound;
    }
}
