using Markdown.Dto;
using Markdown.TagUtils.Abstractions;

namespace Markdown.TagUtils.Implementations;

public class EscapeStrategy(ITagContext context) : BaseTagStrategy(context)
{
    public override void Process(Token token)
    {
        context.Append(token.Value);
        context.SkipNextAsMarkup = true;
    }
}