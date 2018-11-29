using System;

namespace Markdown
{
    public class PTagInfo : ITagInfo
    {
        public Predicate<CollectionView<char>> StartCondition => w => false;
        public Predicate<CollectionView<char>> EndCondition => w => false;
        public Action<TagReader> OnTagStart { get; }
        public Action<TagReader> OnTagEnd { get; }
        public string HtmlTagText => "p";
        public string MarkdownTagText => "";
        public int TagLength => 0;
        public Token GetNewToken(int position)
        {
            return new Token(position, this);
        }
    }
}
