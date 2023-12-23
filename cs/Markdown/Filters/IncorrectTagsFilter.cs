using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Filters;

internal class IncorrectTagsFilter : FilterBase
{
    private readonly string text;

    public IncorrectTagsFilter(FilterBase? nextFilter, string text) : base(nextFilter)
    {
        this.text = text;
    }

    public override IList<IToken> Filter(IList<IToken> tokens)
    {
        var result = new List<IToken>();
        foreach (var token in tokens)
        {
            result.Add(token);
            if (token.Type == TokenType.Text) continue;
            if (!SupportedTags.IsValidTokenTag(token, text))
                token.Type = TokenType.Text;
        }
        if (nextFilter != null)
            return nextFilter.Filter(result);
        return result;
    }
}
