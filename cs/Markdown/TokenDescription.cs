using System;

namespace Markdown
{
    public class TokenDescription
    {
        public Func<string, int, Token> ReadToken;

        public TokenDescription(Func<string, int, Token> readToken)
        {
            ReadToken = readToken;
        }

        public bool TryReadToken(string text, int position, out Token token)
        {
            token = ReadToken(text, position);
            return token.IsEmpty;
        } 
    }
}
