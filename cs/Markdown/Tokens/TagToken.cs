using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;

namespace Markdown.Tokens
{
    public class TagToken : Token
    {
        public SubTagOrder Order { get; }

        public TagToken(TokenType type, int start, int length, SubTagOrder order) 
            : base(type, start, length)
        {
            Order = order;
        }
    }
}
