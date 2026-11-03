using Markdown.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.MDNodes
{
    public class TextNode : MarkdownNode
    {
        public string Content { get; set; }

        public TextNode(string content) => Content = content;

        public override string Render() => Content;
    }
}
