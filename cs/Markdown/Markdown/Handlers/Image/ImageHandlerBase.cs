using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Image;

public abstract class ImageHandlerBase : ITokenHandler
{
    public abstract IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens);
    
    protected static bool IsImageTag(IToken token) => token.TagType is TagType.Image;
}