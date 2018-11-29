using System;

namespace Markdown
{
    public class StrongTagInfo : ITagInfo
    {
        public Predicate<CollectionView<char>> StartCondition =>
            w => !w.IsAlphanumeric(-1)
                 && w.IsUnderscore(0)
                 && w.IsUnderscore(1)
                 && !w.IsWhiteSpace(2);
        public Predicate<CollectionView<char>> EndCondition =>
            w => !w.IsWhiteSpace(-1)
            && w.IsUnderscore(0)
            && w.IsUnderscore(1)
            && !w.IsAlphanumeric(2);

        public Action<TagReader> OnTagStart =>
            t => t.Skip(TagLength);

        public Action<TagReader> OnTagEnd =>
            t => t.Skip(TagLength);
        public string HtmlTagText => "strong";
        public string MarkdownTagText => "__";
        public int TagLength => 2;
        public Token GetNewToken(int position)
        {
            return new Token(position, this);
        }
    }
}
