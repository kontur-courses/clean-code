using System.Collections.Generic;
using Markdown.TagEvents;

namespace Markdown.TagParsers
{
    public interface ITagParser
    {
        List<TagEvent> Parse();
    }
}
