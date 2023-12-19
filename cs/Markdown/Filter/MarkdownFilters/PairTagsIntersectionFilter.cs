using Markdown.Filter.Utils;
using Markdown.Tokens;
using Markdown.Tokens.Decorators;
using Markdown.Tokens.Utils;

namespace Markdown.Filter.MarkdownFilters;

public class PairTagsIntersectionFilter : TokenFilterChain
{
    private static bool DifferentPairsIntersect(Token token1, Token token2, Token token3, Token token4)
    {
        return TokenUtils.TokenTypeEqualityComparer.Equals(token1.Type, token3.Type)
               && TokenUtils.TokenTypeEqualityComparer.Equals(token2.Type, token4.Type)
               && !TokenUtils.TokenTypeEqualityComparer.Equals(token1.Type, token2.Type)
               && !token1.IsClosingTag
               && !token2.IsClosingTag
               && token3.IsClosingTag
               && token4.IsClosingTag;
    }
    
    public override List<TokenFilteringDecorator> Handle(List<TokenFilteringDecorator> tokens, string line)
    {
        var pairTags = FilteringUtils.GetPairedTokens(tokens);

        for (var i = 0; i < pairTags.Count - 3; i++)
        {
            if (!DifferentPairsIntersect(pairTags[i], pairTags[i + 1], pairTags[i + 2], pairTags[i + 3]))
                continue;

            pairTags[i].IsMarkedForDeletion = true;
            pairTags[i + 1].IsMarkedForDeletion = true;
            pairTags[i + 2].IsMarkedForDeletion = true;
            pairTags[i + 3].IsMarkedForDeletion = true;
        }

        return base.Handle(FilteringUtils.DeleteMarkedTokens(tokens), line);
    }
}