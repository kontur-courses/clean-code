using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public readonly int Position;
        public readonly int Length;
        public readonly string Value;
        public readonly TagType TagType;

        public Token(int position, string value, TagType tagType)
        {
            Position = position;
            Value = value;
            TagType = tagType;
            Length = GetTokenLength(value, TagType);
        }

        public string ConvertToHtml()
        {
            throw new NotImplementedException();
        }
        
        private int GetTokenLength(string value, TagType tagType)
        {
            throw new NotImplementedException();
        }
    }
}