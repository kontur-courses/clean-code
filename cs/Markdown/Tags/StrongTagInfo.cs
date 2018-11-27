using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class StrongTagInfo : ITagInfo
    {
        public Predicate<StringView> StartCondition =>
            w => w[0] == '_'
            && w[1] == '_'
            && !char.IsWhiteSpace(w[2]);
        public Predicate<StringView> EndCondition =>
            w => w[0] == '_'
            && w[1] == '_';

        public Action<TagReader> OnTagStart =>
            t => t.SkipAndAdd(TagLength);

        public Action<TagReader> OnTagEnd =>
            t => t.SkipAndAdd(TagLength);
        public string HtmlTagText => "strong";
        public int TagLength => 2;
        public Token GetNewToken(int position)
        {
            return new Token(position, this);
        }
    }
}
