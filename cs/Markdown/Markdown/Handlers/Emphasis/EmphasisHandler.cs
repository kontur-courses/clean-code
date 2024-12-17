using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class EmphasisHandler : EmphasisHandlerBase
{
    private readonly EmphasisHandlerBase[] emphasisHandlers =
    [
        new PairEmphasisHandler(),
        new SkipEmphasisHandler(),
        new SeriesEmphasisHandler(),
        new IntersectEmphasisHandler(),
        new NestedEmphasisHandler(),
        new TextInsideEmphasisHandler(),
        new DifferentWordsEmphasisHandler(),
        new NonPairEmphasisTagHandler()
    ];

    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        var handledTokens = emphasisHandlers.Aggregate(tokens,
            (current, emphasisHandler) => emphasisHandler.Handle(current));
        return handledTokens;
    }
}