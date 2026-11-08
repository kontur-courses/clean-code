using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Abstractions
{
    public abstract class BlockNode : MarkdownNode
    {
        public string RawContent { get; set; }
        public List<InlineNode> Children { get; set; } = new List<InlineNode>();
    }
}
