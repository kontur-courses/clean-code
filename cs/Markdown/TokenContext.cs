using System.Collections.Generic;

namespace Markdown
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