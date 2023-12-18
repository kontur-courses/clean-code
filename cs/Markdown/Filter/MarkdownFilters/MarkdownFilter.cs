using Markdown.Tokens;
using Markdown.Tokens.Types;

namespace Markdown.Filter.MarkdownFilters;

public class MarkdownFilter : TokenFilterBase
{
    public override List<Token> FilterTokens(List<Token> tokens, string line)
    {
        var chain = new TokenFilterChain();
        
        chain.SetNext(new EmptyLinesFilter())
            .SetNext(new BreakingNumbersFilter())
            .SetNext(new DifferentWordsFilter())
            .SetNext(new NestedFilter(new StrongToken(), new EmphasisToken()))
            .SetNext(new SpaceInterruptionFilter())
            .SetNext(new PairTagsIntersectionFilter())
            .SetNext(new UnpairedTagsFilter());
        
        var result = chain.Handle(PackTokensForFiltering(tokens), line);
        
        return UnpackFilteredTokens(result);
    }
}