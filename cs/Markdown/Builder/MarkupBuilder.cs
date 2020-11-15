using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tag;

namespace Markdown.Builder
{
    public class MarkupBuilder : IMarkupBuilder
    {
        private readonly HashSet<ITagData> tags;
        
        public MarkupBuilder(params ITagData[] tags)
        {
            this.tags = tags.ToHashSet();
        }
        
        public string Build(List<TextToken> textTokens)
        {
            throw new NotImplementedException();
        }
    }
}