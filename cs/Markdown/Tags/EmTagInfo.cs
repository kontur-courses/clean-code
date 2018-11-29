using System;

namespace Markdown
{
    public class EmTagInfo : ITagInfo
    {
        public Predicate<CollectionView<char>> StartCondition =>
            w => w.Element(-1).IsNot(char.IsLetterOrDigit)
                 && w.Element(0).Is('_')
                 && w.Element(1).IsNot(char.IsWhiteSpace);

        public Predicate<CollectionView<char>> EndCondition =>
            w => w.Element(-1).IsNot(char.IsWhiteSpace)
                 && w.Element(0).Is('_')
                 && w.Element(1).IsNot('_')
                 && w.Element(1).IsNot(char.IsLetterOrDigit);

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
