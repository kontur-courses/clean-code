using Markdown.Tokens;
using Markdown.Tokens.Utils;

namespace Markdown.Filter.MarkdownFilters;

public class UnpairedTagsFilter : TokenFilterChain
{
    public override List<Token> Handle(List<Token> tokens, string line)
    {
        var pairTags = TokenUtils.GetPairedTokens(tokens);

        for (var i = 0; i < pairTags.Count; i++)
        {
            var pair = TokenUtils.FindTokenPair(i, pairTags);
            if (pair is null)
                pairTags[i].IsMarkedForDeletion = true;
        }

        return base.Handle(TokenUtils.DeleteMarkedTokens(tokens), line);
    }
}