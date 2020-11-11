using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MarkupParser : IParser
    {
        private readonly HashSet<ITagData> tags;
        
        public MarkupParser(params ITagData[] tags)
        {
            this.tags = tags.ToHashSet();
        }

        public List<TextToken> Parse(string text)
        {
            throw new NotImplementedException();
        }
    }
}