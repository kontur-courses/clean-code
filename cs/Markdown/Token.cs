using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Token
    {
        public string Value;
        public bool IsTaged;

        public Token(string value, bool taged)
        {
            this.Value = value;
            this.IsTaged = taged;
        }
    }

    public static class TokenExtensions
    {
        public static bool IsOpen(this List<Token> tokens, int position)
        {
            if (position < 0 || position >= tokens.Count - 1)
                return false;
            return !string.IsNullOrWhiteSpace(tokens[position + 1].Value);

        }
        public static bool IsClose(this List<Token> tokens, int position)
        {
            if (position <= 0 || position > tokens.Count)
                return false;
            return !string.IsNullOrWhiteSpace(tokens[position - 1].Value);
        }
        
    }
}