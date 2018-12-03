using System.Runtime.Remoting.Messaging;

namespace Markdown
{
    public class Token
    {
        public string Value { get; set; }
        public TokenType Type { get; }
        
        public Token(string value, TokenType type)
        {
            Value = value;
            Type = type;
        }
    }
}
