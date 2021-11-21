using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.TokenParser
{
    public class TokenContext
    {
        public Token Token { get; }
        public List<TokenNode> Children { get; } = new();

        public TokenContext(Token token)
        {
            Token = token;
        }
    }
}