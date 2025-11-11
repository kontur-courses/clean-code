using System.Text;
using Markdown.Dto;
using Markdown.TagUtils.Abstractions;

namespace Markdown.TagUtils.Implementations;

public class ItalicStrategy(ITagContext context) : BaseTagStrategy(context)
{
    public override void Process(Token token)
    {
        ProcessTag(token, false, (parent, t) =>
            CloseContext.IsInvalidUnderScoreCloseContext(parent, t, token.Value.Length));
    }
}