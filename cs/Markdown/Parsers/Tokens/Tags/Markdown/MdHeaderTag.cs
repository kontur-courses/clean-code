using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdHeaderTag : Tag
    {
        public MdHeaderTag() : base("#")
        {

        }
        public override bool IsValidTag(string data, int position)
        {
            position++;
            return data.Length > position && char.IsWhiteSpace(data, position);
        }
    }
}
