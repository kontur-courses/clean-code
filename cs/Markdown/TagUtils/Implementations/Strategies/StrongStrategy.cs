using Markdown.Dto;
using Markdown.TagUtils.Abstractions;

namespace Markdown.TagUtils.Implementations;

public class StrongStrategy(ITagContext context) : BaseTagStrategy(context)
{
    public override void Process(Token token)
    {
        ProcessTag(token, true, (parent, t) =>
            CloseContext.IsInvalidUnderScoreCloseContext(parent, t, token.Value.Length));
    }
}