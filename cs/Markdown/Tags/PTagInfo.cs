using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class PTagInfo : TagInfo
    {
        public override Predicate<Window> StartCondition => w => false;
        public override Predicate<Window> EndCondition => w => false;
        public override Action<TagReader> OnTagStart { get; }
        public override Action<TagReader> OnTagEnd { get; }
        public override string TagText => "p";
        public override int TagLength => 0;
    }
}
