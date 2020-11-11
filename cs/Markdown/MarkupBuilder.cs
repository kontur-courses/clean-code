using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MarkupBuilder : IBuilder
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