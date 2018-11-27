using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class StrongTagInfo : TagInfo
    {
        public override Predicate<StringView> StartCondition =>
            w => w[0] == '_'
            && w[1] == '_'
            && !char.IsWhiteSpace(w[2]);
        public override Predicate<StringView> EndCondition =>
            w => w[0] == '_'
            && w[1] == '_';

        public override Action<TagReader> OnTagStart =>
            t => t.SkipAndAdd(TagLength);

        public override Action<TagReader> OnTagEnd =>
            t => t.SkipAndAdd(TagLength);
        public override string HtmlTagText => "strong";
        public override int TagLength => 2;
    }
}
