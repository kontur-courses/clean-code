using System;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class TokenNode
    {
        public readonly Token Token;
        public readonly TokenNode[] Children;

        public TokenNode(Token token, TokenNode[] children)
        {
            Token = token;
            Children = children;
        }

        public TokenNode(Token token, TokenNode child) : this(token, new[] { child })
        {
        }

        public TokenNode(Token token) : this(token, Array.Empty<TokenNode>())
        {
        }

        public override bool Equals(object? obj) => obj is TokenNode node && Equals(node);

        private bool Equals(TokenNode node)
        {
            return Token.Equals(node.Token) && Children.Length == node.Children.Length &&
                   !Children.Where((t, i) => !t.Equals(node.Children[i])).Any();
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Token.GetHashCode());
            foreach (var child in Children) hash.Add(child.GetHashCode());

            return hash.ToHashCode();
        }

        public override string ToString() => ToString(0);

        private string ToString(int nesting)
        {
            var sb = new StringBuilder();
            var tab = new string('\t', nesting);
            sb.AppendLine(tab);
            sb.AppendLine($"{tab}{{");
            sb.AppendLine($"{tab}\tToken = {ToString(Token)}");
            foreach (var child in Children)
            {
                sb.AppendLine($"{tab}\tChildren = ");
                sb.Append($"{tab}\t[");
                sb.Append($"{tab}\t{child.ToString(nesting + 1)}");
                sb.AppendLine($"{tab}\t]");
            }

            sb.AppendLine($"{tab}}}");
            return sb.ToString();
        }

        private string ToString(Token token) => $"{{ TokenType = {token.Type}, Value = \"{token.Value}\" }}";
    }
}