using System;

namespace Markdown
{
    public class EmTagInfo : ITagInfo
    {
        public Predicate<CollectionView<char>> StartCondition =>
            w => !w.IsAlphanumeric(-1)
                 && w.IsUnderscore(0)
                 && !w.IsUnderscore(1)
                 && !w.IsWhiteSpace(1);

        public Predicate<CollectionView<char>> EndCondition =>
            w => !w.IsWhiteSpace(-1)
                 && !w.IsUnderscore(-1)
                 && w.IsUnderscore(0)
                 && !w.IsUnderscore(1)
                 && !w.IsAlphanumeric(1);

        public Action<TagReader> OnTagStart =>
            t => t.Skip(TagLength);

        public Action<TagReader> OnTagEnd =>
            t => t.Skip(TagLength);
        public string HtmlTagText => "em";
        public string MarkdownTagText => "_";
        public int TagLength => 1;
        public Token GetNewToken(int position)
        {
            return new Token(position, this);
        }
    }
}
