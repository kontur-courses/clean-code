using Markdown.Dto;
using Markdown.TagUtils.Abstractions;

namespace Markdown.TagUtils.Implementations;

public class LinkStrategy(ITagContext context) : BaseTagStrategy(context)
{
    public override void Process(Token token)
    {
        ProcessTag(token, false, (parent, t) =>
            CloseContext.IsInvalidLinkCloseContext(parent, t, token.Value.Length));
    }
}