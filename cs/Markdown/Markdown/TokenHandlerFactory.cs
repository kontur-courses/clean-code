using Markdown.Markdown.Handlers;
using Markdown.Markdown.Handlers.Emphasis;
using Markdown.Markdown.Handlers.Image;

namespace Markdown.Markdown;

public static class TokenHandlerFactory
{
    private static readonly ITokenHandler[] Handlers =
    [
        new TagTypeHandler(),
        new EscapeSequenceHandler(),
        new EmphasisHandler(),
        new HeaderHandler(),
        new ImageHandler()
    ];

    public static IEnumerable<ITokenHandler> CreateHandlers() => Handlers;
}