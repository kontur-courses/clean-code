using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tag;

namespace Markdown.Parser
{
    public class MarkupParser : IMarkupParser
    {
        private readonly Dictionary<string, TagData> openKeyToTag;
        private readonly Dictionary<string, TagData> closeKeyToTag;
        private readonly PrefixTree openTagTree;
        private readonly PrefixTree closeTagTree;
        
        public MarkupParser(params ITagData[] tags)
        {
            openTagTree = new PrefixTree(tags
                .Select(tag => tag.IncomingBorder.Open).ToList());
            closeTagTree = new PrefixTree(tags
                .Select(tag => tag.IncomingBorder.Close).ToList());
        }

        public List<TextToken> Parse(string text)
        {
            foreach (var character in text)
            {
                
            }
            throw new NotImplementedException();
        }
    }
}