using System;

namespace Markdown
{
    public class StrongTagInfo : ITagInfo
    {
        public Predicate<StringView> StartCondition =>
            w => w[0] == '_'
            && w[1] == '_'
            && !char.IsWhiteSpace(w[2]);
        public Predicate<StringView> EndCondition =>
            w => !char.IsWhiteSpace(w[-1])
            && w[0] == '_'
            && w[1] == '_';

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
