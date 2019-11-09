using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token
    {
        public TokenType tokenType;
        public Token parent;
        public List<Token> nestedTokens;

        public readonly int position;
        public int length;
        public readonly string text;
        public readonly int nestingLevel;

        public Token(string text, 
            int position,
            TokenType tokenType = TokenType.Raw,
            int length = 0, 
            int nestingLevel = 0,
            Token parent = null)
        {
            this.tokenType = tokenType;
            this.text = text;
            this.position = position;
            this.nestingLevel = nestingLevel;
            this.parent = parent;

            nestedTokens = new List<Token>();
        }

        public void AddNestedToken(Token token)
        {
            nestedTokens.Add(token);
        }
    }
}
