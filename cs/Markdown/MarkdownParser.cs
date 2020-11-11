using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownParser
    {
        private readonly HashSet<ITagData> tags = new HashSet<ITagData>();
        
        public MarkdownParser(params ITagData[] tags)
        {
            foreach (var tag in tags)
                this.tags.Add(tag);
        }

        public List<TagData> Parse(string text)
        {
            throw new NotImplementedException();
        }
    }
}