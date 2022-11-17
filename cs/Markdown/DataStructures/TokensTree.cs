using System;
using System.Collections.Generic;

namespace Markdown.DataStructures
{
    public class TokensTree : ITokensTree
    {
        public Token RootToken { get; }

        public TokensTree()
        {
            RootToken = new Token(null, null, 0, false);
        }

        public void AddToken(Token parent)
        {
            throw new NotImplementedException();
        }

        public void RemoveToken(Token token)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Token> GetAllTokens(Token token)
        {
            throw new NotImplementedException();
        }
    }
}