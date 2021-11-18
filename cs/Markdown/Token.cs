using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public readonly string Value;
        public List<Token> InnerTokens;

        protected Token(string value, List<Token> innerTokens)
        {
            Value = value;
            InnerTokens = innerTokens;
        }
    }
    
    public class Header : Token
    {
        public Header(string value, List<Token> innerTokens = null) :
            base(value, innerTokens)
        {
        }
    }

    public class Paragraph : Token
    {
        public Paragraph(string value, List<Token> innerTokens = null) :
            base(value, innerTokens)
        {
        }
    }

    public class StrongText : Token
    {
        public StrongText(string value, List<Token> innerTokens = null) :
            base(value, innerTokens)
        {
        }
    }

    public class ItalicText : Token
    {
        public ItalicText(string value, List<Token> innerTokens = null) :
            base(value, innerTokens)
        {
        }
    }
}
