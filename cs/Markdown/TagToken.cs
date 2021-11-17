using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TagToken : Token
    {
        private readonly HTMLTag _tag;

        public TagToken(int begin, int end, HTMLTag tag) : base(begin, end)
        {
            _tag = tag;
        }

        public override string Render(string str)
        {
            return _tag.OpenTag + str.Substring(_begin, Length) + _tag.CloseTag;
        }
    }
}
