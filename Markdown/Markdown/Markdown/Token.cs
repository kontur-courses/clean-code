using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token
    {
        public readonly int Length;
        public readonly TokenType TokensType;
        public readonly int Position;
        public readonly string Value;

        public Token(string value, int position, int length,TokenType type)
        {
            TokensType = type;
            Position = position;
            Length = length;
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if ((Token)obj == null)
                return false;
            return Equals((Token)obj);
        }

        protected bool Equals(Token other)
        {
            return Length == other.Length && Position == other.Position && string.Equals(Value, other.Value);
        }
        public int GetIndexNextToToken()
        {
            return Position + Length;
        }
    }
}
