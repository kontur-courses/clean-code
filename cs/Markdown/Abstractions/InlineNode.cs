using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Abstractions
{
    public abstract class InlineNode : MarkdownNode
    {
        public string Content { get; set; }
        public InlineNode(string content)
        {
            Content = content;
        }
    }
}
