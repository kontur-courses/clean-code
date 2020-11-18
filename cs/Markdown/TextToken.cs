using System.Collections.Generic;
using Markdown.Tag;

namespace Markdown
{
    public class TextToken
    {
        public ITagData Tag { get; }
        public int Start { get; }
        public int End { get; set; }
        public HashSet<TextToken> SubTokens { get; private set;}

        public TextToken(ITagData tokenTag, int startPosition)
        {
            Tag = tokenTag;
            Start = startPosition;
            SubTokens = new HashSet<TextToken>();
        }

        public void AddNestedTokens(params TextToken[] tokensToNested)
        {
            SubTokens.UnionWith(tokensToNested);
        }
        
        public void RemoveNestedTokens(params TextToken[] tokensToRemove)
        {
            foreach (var tokenToRemove in tokensToRemove)
            {
                SubTokens.Remove(tokenToRemove);
            }
        }
    }
}