using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class PTagInfo : TagInfo
    {
        public override Predicate<StringView> StartCondition => w => false;
        public override Predicate<StringView> EndCondition => w => false;
        public override Action<TagReader> OnTagStart { get; }
        public override Action<TagReader> OnTagEnd { get; }
        public override string HtmlTagText => "p";
        public override int TagLength => 0;
    }
}
