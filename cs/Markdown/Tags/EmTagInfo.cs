using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class EmTagInfo : ITagInfo
    {
        public Predicate<StringView> StartCondition =>
            w => w[-1] != '_'
                 && w[0] == '_'
                 && w[1] != '_'
                 && !char.IsWhiteSpace(w[1]);

        public Predicate<StringView> EndCondition =>
            w => w[0] == '_'
            && !char.IsWhiteSpace(w[-1])
            && w[1] != '_';

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
