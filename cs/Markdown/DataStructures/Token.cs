using System;
using System.Collections.Generic;

namespace Markdown.DataStructures
{
    public class Token : IToken
    {
        public ITag Tag { get; }
        public Token Parent { get; set; }
        public List<Token> Children { get; }
        public int StartIndex { get; }
        public int EndIndex { get; set; }
        public bool StartsInsideWord { get; }
        public bool EndsInsideWord { get; set; }

        public bool ContainsSpaces { get; set; }
        public Token(ITag tag, Token parent, int startIndex, bool startsInsideWord = false)
        {
            Children = new List<Token>();
            Tag = tag;
            Parent = parent;
            StartIndex = startIndex;
            StartsInsideWord = startsInsideWord;
        }
    }
}