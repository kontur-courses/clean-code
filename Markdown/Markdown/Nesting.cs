using Markdown.NestingFilters;
using Markdown.Tokens;

namespace Markdown;

public static class Nesting
{
    public static readonly IReadOnlyList<INestingFilter> Filters = new List<INestingFilter>
    {
        new BoldInItalicFilter()
    };

    public static void ExcludeNestedOfType<T>(Token token) where T : Token
    {
        for (var i = 0; i < token.NestedTokens.Count; i++)
        {
            var nested = token.NestedTokens[i];
            if (nested is not T) continue;

            var tokenText = TextToken.ToText(nested);
            token.NestedTokens.Insert(i, tokenText.Opening);

            var ts = token.NestedTokens.Where(x => x is T).ToList();
            token.NestedTokens.RemoveAll(x => x is T);
            token.NestedTokens.ForEach(ExcludeNestedOfType<T>);
            foreach (var t in ts)
            {
                t.Parent = token;
                token.NestedTokens.InsertRange(i + 1, t.NestedTokens);
                i += t.NestedTokens.Count;
            }

            token.NestedTokens.Insert(i + 1, tokenText.Ending);
        }
    }

    public static void AddToToken(Token token, Token wrapper)
    {
        wrapper.Parent = token.Parent;
        token.Parent = wrapper;
        wrapper.NestedTokens.Add(token);
    }
}