using Markdown.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.MDNodes
{
    public abstract class PairedInlineNode : InlineNode
    {
        protected abstract string OpeningTag { get; }
        protected abstract string ClosingTag { get; }

        public List<InlineNode> Children { get; protected set; } = new();

        public override string Render()
        {
            var inner = string.Join("", Children.Select(c => c.Render()));
            return $"{OpeningTag}{inner}{ClosingTag}";
        }
    }
}
