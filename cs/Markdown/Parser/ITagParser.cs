using System.Collections.Generic;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public interface ITagParser
    {
        List<TypedToken> Parse(string text, ITag tag);
    }
}
