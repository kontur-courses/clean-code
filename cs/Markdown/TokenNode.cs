using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TokenNode
    {
        public readonly Token root;
        public readonly int nestingLevel;
        public readonly List<TokenNode> nestedNodes;

        public TokenNode(Token root, int nestingLevel = 0)
        {
            this.root = root;
            this.nestingLevel = nestingLevel;
            nestedNodes = new List<TokenNode>();
        }
    }
}
