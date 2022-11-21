using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Enums;

namespace Markdown.Tokens
{
    public class Tag:Token
    {
        public TagStatus Status { get; set; }
        public Tag(int start, int end, TokenType type, TagStatus status) : base(start, end, type)
        {
            Status = status;
        }
    }
}
