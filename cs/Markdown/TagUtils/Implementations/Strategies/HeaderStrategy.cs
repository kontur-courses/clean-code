using System.Text;
using Markdown.Dto;
using Markdown.TagUtils.Abstractions;

namespace Markdown.TagUtils.Implementations;

public class HeaderStrategy(ITagContext context) : BaseTagStrategy(context)
{
    public override void Process(Token token)
    {
        context.Open(new Tag(token, new StringBuilder(token.Value), true));
    }
}