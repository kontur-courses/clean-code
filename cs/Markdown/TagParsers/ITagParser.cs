using System.Collections.Generic;
using Markdown.TagEvents;
using NUnit.Framework;

namespace Markdown.TagParsers
{
    public interface ITagParser
    {
        List<TagEvent> Parse();
    }
}
