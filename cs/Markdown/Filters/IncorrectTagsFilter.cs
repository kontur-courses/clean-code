using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Filters;

internal class IncorrectTagsFilter : IFilter
{
    private readonly string text;

    public IncorrectTagsFilter(string text)
    {
        this.text = text;
    }

    public int Order { get; } = 1;

    public IList<IToken> Filter(IList<IToken> tokens)
    {
        var result = new List<IToken>();
        foreach (var token in tokens)
        {
            result.Add(token);
            if (token.Type == TokenType.Text) continue;
            if (!SupportedTags.IsValidTokenTag(token, text))
                token.Type = TokenType.Text;
        }
        return result;
    }
}
