using System;

namespace Markdown
{
    public class StrongTagInfo : ITagInfo
    {
        public Predicate<CollectionView<char>> StartCondition =>
            w => w.Element(-1).IsNot(char.IsLetterOrDigit)
                 && w.Element(0).Is('_')
                 && w.Element(1).Is('_')
                 && w.Element(2).IsNot(char.IsWhiteSpace);

        public Predicate<CollectionView<char>> EndCondition =>
            w => w.Element(-1).IsNot(char.IsWhiteSpace)
                 && w.Element(0).Is('_')
                 && w.Element(1).Is('_')
                 && w.Element(2).IsNot(char.IsLetterOrDigit);

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
