using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Filters;

internal class NestedTokensFilter : IFilter
{
    private readonly TagType outer;
    private readonly HashSet<TagType> nested;

    public NestedTokensFilter(TagType outer, HashSet<TagType> nested)
    {
        this.outer = outer;
        this.nested = nested;
    }

    public int Order { get; } = 4;

    public IList<IToken> Filter(IList<IToken> tokens)
    {
        var result = new List<IToken>(tokens);
        var tagTokens = GetAllTagTokens(tokens);
        for (var i = 0; i < tagTokens.Count; i++)
        {
            var token = tagTokens[i];
            var tokenType = TokenUtilities.GetTokenTagType(token);
            if (tokenType == outer)
            {
                var nestedTokens = GetNestedTokens(tagTokens, ref i, tokenType);
                ChangeTypesToTextForTagType(nestedTokens, nested);
            }
        }
        return result;
    }

    private IList<IToken> GetNestedTokens(IList<IToken> tagTokens, ref int index, TagType? tokenType)
    {
        var result = new List<IToken>();
        for (var i = index + 1; i < tagTokens.Count; i++)
        {
            var token = tagTokens[i];
            var type = TokenUtilities.GetTokenTagType(token);
            if (type == tokenType)
            {
                index = i + 1;
                break;
            }
            else
            {
                result.Add(token);
            }
        }

        return result;
    }

    private void ChangeTypesToTextForTagType(IEnumerable<IToken> tokens, HashSet<TagType> types)
    {
        foreach (var token in tokens)
        {
            var tokenTagType = TokenUtilities.GetTokenTagType(token);
            if (tokenTagType != null && types.Contains((TagType)tokenTagType))
                token.Type = TokenType.Text;
        }
    }

    private IList<IToken> GetAllTagTokens(IEnumerable<IToken> tokens)
    {
        return tokens.Where(token => token.Type == TokenType.Tag).ToList();
    }
}
