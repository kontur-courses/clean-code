using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public string Value;
        public List<Token> InnerTokens = new List<Token>();
        public bool Closed;
        public bool Valid = true;
        public bool HaveInner => InnerTokens != null && InnerTokens.Count > 0;

        public Token()
        {
        }

        public Token(string value)
        {
            Value = value;
            Closed = true;
        }
    }
    
    public class Header : Token
    {
        public Header(string value, List<Token> innerTokens = null) :
            base(value)
        {
        }
    }

    public class Paragraph : Token
    {
        public Paragraph(string value, List<Token> innerTokens = null) :
            base(value)
        {
        }
    }

    public class StrongText : Token
    {
    }

    public class ItalicText : Token
    {
    }
}
