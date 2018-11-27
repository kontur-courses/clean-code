using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public abstract class TagInfo
    {
        public abstract Predicate<StringView> StartCondition { get; }
        public abstract Predicate<StringView> EndCondition { get; }
        public abstract Action<TagReader> OnTagStart { get; }
        public abstract Action<TagReader> OnTagEnd { get; }
        public abstract string HtmlTagText { get; }
        public abstract int TagLength { get; }

        public Token GetNewToken(int position)
        {
            return new Token(position, this);
        }
    }
}
