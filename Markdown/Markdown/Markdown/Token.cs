using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token:IToken
    {
      
        public TokenType TokensType;
        public int Position { get; set; }
        public int Length { get; set; }

        public Token(int position, int length)
        {
            Position = position;
            Length = length;
        }
        public Token(int position, int length,TokenType type)
        {
            Position = position;
            Length = length;
            TokensType=type;
        }

        public override bool Equals(object obj)
        {
            if ((Token)obj == null)
                return false;
            return Equals((Token)obj);
        }

       
        public int GetIndexNextToToken()
        {
            return Position + Length;
        }
    }
}
