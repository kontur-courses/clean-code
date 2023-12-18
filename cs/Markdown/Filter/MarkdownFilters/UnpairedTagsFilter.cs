using Markdown.Tokens.Decorators;
using Markdown.Tokens.Utils;

namespace Markdown.Filter.MarkdownFilters;

public class UnpairedTagsFilter : TokenFilterChain
{
    public override List<TokenFilteringDecorator> Handle(List<TokenFilteringDecorator> tokens, string line)
    {
        var pairTags = FilteringUtils.GetPairedTokens(tokens);

        for (var i = 0; i < pairTags.Count; i++)
        {
            var pair = TokenUtils.FindTokenPair(i, pairTags);
            if (pair is null)
                pairTags[i].IsMarkedForDeletion = true;
        }

        return base.Handle(FilteringUtils.DeleteMarkedTokens(tokens), line);
    }
}