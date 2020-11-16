using System;
using System.Collections.Generic;
using Markdown.Infrastructure.Blocks;

namespace Markdown.Infrastructure.Parsers
{
    public interface ITagParser
    {
        public IEnumerable<TagInfo> ParseTags(string text);
    }
}