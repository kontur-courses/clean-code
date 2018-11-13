using System.Collections.Generic;

namespace Markdown
{
    public class TokenTreeNode
    {
        public readonly TokenType Type;
        public readonly string Text;
        public readonly List<TokenTreeNode> Children = new List<TokenTreeNode>();
        public bool IsRaw;

        public TokenTreeNode(TokenType type = TokenType.Text, string text = null)
        {
            Type = type;
            Text = text;
        }
    }
}