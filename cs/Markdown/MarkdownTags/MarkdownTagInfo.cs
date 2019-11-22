using System.Collections.Generic;

namespace Markdown.MarkdownTags
{
    internal abstract class MarkdownTagInfo
    {
        public abstract string MarkdownTagDesignation { get; }
        public abstract string HtmlTagDesignation { get; }
        public abstract int Priority { get; }
        
        public virtual bool IsOpeningTag(char previousSymbol, char nextSymbol, HashSet<char> specialSymbols) =>
            char.IsWhiteSpace(previousSymbol)
            && !char.IsWhiteSpace(nextSymbol)
            && !specialSymbols.Contains(previousSymbol) 
            && !specialSymbols.Contains(nextSymbol);

        public virtual bool IsClosingTag(char previousSymbol, char nextSymbol, HashSet<char> specialSymbols) =>
            char.IsWhiteSpace(nextSymbol)
            && !char.IsWhiteSpace(previousSymbol)
            && !specialSymbols.Contains(nextSymbol)
            && !specialSymbols.Contains(previousSymbol);
    }
}
