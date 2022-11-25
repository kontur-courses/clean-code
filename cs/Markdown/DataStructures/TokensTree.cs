using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.DataStructures
{
    public class TokensTree : ITokensTree
    {
        public Token RootToken { get; }

        public TokensTree()
        {
            RootToken = new Token(null, null, 0, false);
        }

        public void AddToken(Token token)
        {
            token.Parent.Children.Add(token);
        }

        public void RemoveToken(Token token)
        {
            if (token.Children.Count > 0)
                foreach (var child in token.Children)
                    token.Parent.Children.Add(child);
            token.Parent.Children.Remove(token);
        }

    }
}