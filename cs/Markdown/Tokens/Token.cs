using System.Collections.Generic;

namespace Markdown.Tokens
{
    internal abstract class Token
    {
        public Token Parent { get; set; } = null;

        public List<Token> Children { get; } = new List<Token>();

        public int BeginIndex { get; set; }
        public int EndIndex { get; set; }

        public abstract string MarkdownTag { get; }

        public bool MatchTag(int i, ref string document)
        {
            var tagLength = MarkdownTag.Length;
            return document.TryGetSubstring(i, tagLength, out string substr) &&
                substr.Equals(MarkdownTag);
        }

        public virtual bool CanBeBeginned(int i, ref string document, Token parentToken)
        {
            BeginIndex = i;
            EndIndex = i + MarkdownTag.Length;
            return true;
        }
    }
}
