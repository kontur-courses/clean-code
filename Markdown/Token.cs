using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    internal enum TokenType {
        Text, 
        Underscore,
        DoubleUnderscore,
        Digit,
        NewLine,
        Grid,
        Whitespace,
        Tag,
        Backslash,

    }
    internal class Token
    {
        public TokenType Type;
        public bool CanOpen;
        public bool CanClose;
        public string Text;   

        public Token(TokenType kind, bool canOpen, bool canClose, string text)
        {
            Type = kind;
            CanOpen = canOpen;
            CanClose = canClose;
            Text = text;
        }
    }
}
