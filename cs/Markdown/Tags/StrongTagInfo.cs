using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class StrongTagInfo : TagInfo
    {
        public override Predicate<Window> StartCondition =>
            w => w[0] == '_'
            && w[1] == '_';
        public override Predicate<Window> EndCondition =>
            w => w[0] == '_'
            && w[1] == '_';

        public override Action<TagReader> OnTagStart =>
            t => t.SkipAndAdd(TagLength);

        public override Action<TagReader> OnTagEnd =>
            t => t.SkipAndAdd(TagLength);
        public override string TagText => "strong";
        public override int TagLength => 2;

        public override Token GetNewToken(int position)
		{
            return new Token(position, this);
        }
    }
}
