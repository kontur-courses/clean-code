using Markdown.Dto;
using Markdown.TagUtils.Abstractions;

namespace Markdown.TagUtils.Implementations;

public class EndStrategy(ITagContext context) : BaseTagStrategy(context)
{
    public override void Process(Token token)
    {
        while (context.Tags.Count > 0)
        {
            context.CloseTop(true);
        }
    }
}