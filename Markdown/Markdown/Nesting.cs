using Markdown.NestingFilters;
using Markdown.Tokens;

namespace Markdown;

public static class Nesting
{
    public static readonly IReadOnlyList<INestingFilter> Filters = new List<INestingFilter>
    {
        new BoldInItalicFilter()
    };
    
    public static void ExcludeNestedOfType<T>(Token token) where  T : Token
    {
        for (var i = 0; i < token.NestedTokens.Count; i++)
        {
            var nested = token.NestedTokens[i];
            if (nested is not T) continue;
            token.NestedTokens.Insert(i, new TextToken { FirstPosition = nested.FirstPosition, Length = nested.Opening.Length });

            var ts = token.NestedTokens.Where(x => x is T).ToList();
            token.NestedTokens.RemoveAll(x => x is T);
            token.NestedTokens.ForEach(ExcludeNestedOfType<T>);
            foreach (var t in ts)
            {
                token.NestedTokens.InsertRange(i + 1, t.NestedTokens);
                i += t.NestedTokens.Count;
            }
            token.NestedTokens.Insert(i + 1, new TextToken
            {
                FirstPosition = nested.LastPosition - nested.Opening.Length + (nested.Opening.Length == 0 ? 0 : 1), 
                Length = nested.Ending.Length
            });
        }
    }
}