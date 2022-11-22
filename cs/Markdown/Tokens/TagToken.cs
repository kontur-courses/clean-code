using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;

namespace Markdown.Tokens
{
    public class TagToken : TypedToken
    {
        public SubTagOrder Order { get; }

        public TagType TagType { get; }

        public TagToken(int start, int length, TagType tagType, SubTagOrder order) 
            : base(start, length, TokenType.Tag)
        {
            TagType = tagType;
            Order = order;
        }
    }
}
