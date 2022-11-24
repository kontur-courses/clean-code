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
        token.NestedTokens.RemoveAll(x => x is T);
        token.NestedTokens.ForEach(ExcludeNestedOfType<T>);
    }
}