using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Domain.Tags;

namespace Markdown.Domain
{
    public class Token
    {
        public Tag TagType { get; }
        public int StartIndex { get; }
        public int EndIndex { get; }
        public bool IsSingleTag { get; }

        public Token(Tag type, int startIndex, int endIndex, bool isSingleTag = false)
        {
            TagType = type;
            IsSingleTag = isSingleTag;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }
}
