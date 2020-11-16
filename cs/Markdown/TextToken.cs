using System.Collections.Generic;
using Markdown.Tag;

namespace Markdown
{
    public class TextToken
    {
        public ITagData Tag { get; }
        public int Start { get; }
        public int End { get; set; }
        public List<TextToken> SubTokens { get; private set;}

        public bool IsValid => Start < End;

        public TextToken(ITagData tokenTag, int startPosition)
        {
            Tag = tokenTag;
            Start = startPosition;
            SubTokens = new List<TextToken>();
        }

        public void AddNestedTokens(params TextToken[] nestedToken)
        {
            SubTokens.AddRange(nestedToken);
        }
    }
}