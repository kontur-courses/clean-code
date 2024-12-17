using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Image;

public class ImageHandler : ImageHandlerBase
{
    private readonly ImageHandlerBase[] imageHandlers =
    [
        new PairImageHandler(),
        new NonPairImageHandler(),
        new UnionImageHandler()
    ];

    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        var handledTokens = imageHandlers.Aggregate(tokens,
            (current, imageHandler) => imageHandler.Handle(current));
        return handledTokens;
    }
}