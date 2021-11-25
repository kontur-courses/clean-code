using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public string Value;
        public List<Token> InnerTokens = new List<Token>();
        public bool Closed;
        public bool InText;
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
}
