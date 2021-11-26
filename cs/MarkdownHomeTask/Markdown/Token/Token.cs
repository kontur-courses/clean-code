using System.Runtime.Remoting.Channels;

namespace Markdown
{
    public class Token
    {
        public readonly int Start;
        public TokenType Type { get; protected set; }
        public readonly string Value;


        public Token(int start, string value, TokenType type)
        {
            Start = start;
            Value = value;
            Type = type;
        }

        public bool IsEmpty => string.IsNullOrEmpty(Value);

        public bool IsTag => Type == TokenType.Tag;

        public bool IsWhiteSpace => Type == TokenType.WhiteSpace;
        public bool IsText => Type == TokenType.Text;

        public Token ToTextToken()
        {
            Type = TokenType.Text;
            return this;
        }

        public override bool Equals(object obj)
        {
            if (obj is Token token)
            {
                return Start == token.Start
                       && Type == token.Type
                       && Value == token.Value;
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Start.GetHashCode() * 18253 + Value.GetHashCode() * 12289 + Type.GetHashCode() * 3559;
            }
        }
    }
}