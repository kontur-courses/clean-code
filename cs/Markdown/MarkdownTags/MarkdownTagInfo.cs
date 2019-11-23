using System.Collections.Generic;

namespace Markdown.MarkdownTags
{
    internal abstract class MarkdownTagInfo
    {
        public abstract string MarkdownTagOpenDesignation { get; }
        public abstract string MarkdownTagCloseDesignation { get; }
        public abstract string HtmlTagDesignation { get; }
        public abstract int Priority { get; }
        
        public virtual bool IsOpeningTag(string tagDesignation, char previousSymbol, char nextSymbol, HashSet<char> specialSymbols) =>
            tagDesignation.Equals(MarkdownTagOpenDesignation)
            && char.IsWhiteSpace(previousSymbol)
            && !char.IsWhiteSpace(nextSymbol)
            && !specialSymbols.Contains(previousSymbol) 
            && !specialSymbols.Contains(nextSymbol);

        public virtual bool IsClosingTag(string tagDesignation, char previousSymbol, char nextSymbol, HashSet<char> specialSymbols) =>
            tagDesignation.Equals(MarkdownTagCloseDesignation)
            && char.IsWhiteSpace(nextSymbol)
            && !char.IsWhiteSpace(previousSymbol)
            && !specialSymbols.Contains(nextSymbol)
            && !specialSymbols.Contains(previousSymbol);
    }
}
