using System.Collections.Generic;

namespace Markdown
{
    internal abstract class MarkdownTag
    {
        public abstract string TagDesignation { get; }
        public abstract string HtmlDesignation { get; }
        public abstract int Priority { get; }
        
        public virtual bool IsOpeningTag(char previousSeparatorSymbol, char nextSeparatorSymbol, HashSet<char> specialSymbols) =>
            char.IsWhiteSpace(previousSeparatorSymbol)
            && !char.IsWhiteSpace(nextSeparatorSymbol)
            && !specialSymbols.Contains(previousSeparatorSymbol) 
            && !specialSymbols.Contains(nextSeparatorSymbol);

        public virtual bool IsClosingTag(char previousSeparatorSymbol, char nextSeparatorSymbol, HashSet<char> specialSymbols) =>
            char.IsWhiteSpace(nextSeparatorSymbol)
            && !char.IsWhiteSpace(previousSeparatorSymbol)
            && !specialSymbols.Contains(nextSeparatorSymbol)
            && !specialSymbols.Contains(previousSeparatorSymbol);
    }
}
