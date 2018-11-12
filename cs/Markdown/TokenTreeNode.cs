using System.Collections.Generic;

namespace Markdown
{
    public class TokenTreeNode
    {
        public TokenType Type;
        public readonly string Text;
        public readonly TokenTreeNode Parent;
        public readonly List<TokenTreeNode> Children = new List<TokenTreeNode>();

        public TokenTreeNode(TokenType type = TokenType.Text, string text = null, TokenTreeNode parent = null)
        {
            Type = type;
            Text = text;
            Parent = parent;
        }

        public void AddChildren(IEnumerable<TokenTreeNode> children)
        {
            foreach (var child in children)
            {
                Children.Add(new TokenTreeNode(child.Type, child.Text, this));
            }
        }
    }
}