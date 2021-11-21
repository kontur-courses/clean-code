using System;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.SyntaxParser
{
    public class TokenTree
    {
        public readonly TokenTree[] Nodes;

        public TokenTree(Token type, params TokenTree[] tokens)
        {
            Token = type;
            Nodes = tokens;
        }

        public Token Token { get; }

        public static TokenTree FromText(string text) => new(Token.Text(text));

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TokenTree) obj);
        }

        private bool Equals(TokenTree node) => Token.Equals(node.Token) && Nodes.SequenceEqual(node.Nodes);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Token.GetHashCode());
            foreach (var child in Nodes)
                hash.Add(child.GetHashCode());

            return hash.ToHashCode();
        }
    }
}