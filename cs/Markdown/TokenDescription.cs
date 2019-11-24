using System;

namespace Markdown
{
    public class TokenDescription
    {
        public Func<string, int, int, Token> ReadToken;

        public TokenDescription(Func<string, int, int, Token> readToken)
        {
            ReadToken = readToken;
        }

        public bool TryReadToken(string text, int position, int index, out Token token)
        {
            token = ReadToken(text, position, index);
            return token.IsEmpty;
        } 
    }
}
