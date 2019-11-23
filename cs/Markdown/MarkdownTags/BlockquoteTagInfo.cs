using System.Collections.Generic;

namespace Markdown.MarkdownTags
{
    internal class BlockquoteTagInfo : MarkdownTagInfo
    {
        public override string MarkdownTagOpenDesignation => ">";
        public override string MarkdownTagCloseDesignation => "\n";
        public override string HtmlTagDesignation => "blockquote";
        public override int Priority => 3;

        public override bool IsOpeningTag(string tagDesignation, char previousSymbol, char nextSymbol, HashSet<char> specialSymbols)
        {
            return tagDesignation.Equals(MarkdownTagOpenDesignation) && previousSymbol == '\n';
        }

        public override bool IsClosingTag(string tagDesignation, char previousSymbol, char nextSymbol, HashSet<char> specialSymbols)
        {
            return tagDesignation.Equals(MarkdownTagCloseDesignation);
        }
    }
}