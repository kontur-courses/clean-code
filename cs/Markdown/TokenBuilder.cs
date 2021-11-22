using System.Text;

namespace Markdown;

internal class TokenBuilder
{
    public StringBuilder Source { get; private set; }
    public int Start { get; private set; }
    public int End { get; private set; }
    public TagSetting? MdTag { get; private set; }
    public TokenBuilder? Parent { get; private set; }

    private readonly List<TokenBuilder> tokens = new();

    public TokenBuilder(StringBuilder source, int start, TokenBuilder? parent)
    {
        Source = source;
        Parent = parent;
        Start = start;
    }

    public TokenBuilder WithEnd(int end)
    {
        End = end;
        return this;
    }

    public TokenBuilder WithMdTag(TagSetting? mdTag)
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
        var actualStart = Start - (Parent?.Start ?? 0);
        if (tokens.Count == 0)
            return Tokenizer.WrapToken(Source.ToString()[Start..End], actualStart, MdTag);

        return BuildCompound(actualStart);
    }

    private CompoundToken BuildCompound(int actualStart)
    {
        var compound = new CompoundToken(Source.ToString()[Start..End], actualStart, MdTag);
        foreach (var token in tokens)
        {
            compound.AddToken(token.Build());
        }

        return compound;
    }
}
