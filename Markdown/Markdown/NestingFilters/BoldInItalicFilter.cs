using Markdown.Tokens;

namespace Markdown.NestingFilters;

public class BoldInItalicFilter : INestingFilter
{
    public void Filter(Token token)
    {
        if (token is ItalicToken)
            Nesting.ExcludeNestedOfType<BoldToken>(token);
        else
        {
            foreach (var t in token.NestedTokens)
                Filter(t);
        }
    }
}