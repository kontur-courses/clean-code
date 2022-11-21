using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Enums;

namespace Markdown.Tokens
{
    public class Text:Token
    {
        public string Value { get; set; }

        public Text(int start, int end, TokenType type, string value) : base(start, end, type)
        {
            Value = value;
        }
    }
}
