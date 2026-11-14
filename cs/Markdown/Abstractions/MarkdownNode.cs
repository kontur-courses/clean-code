using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Abstractions
{
    /// <summary>
    /// Базовый класс для всех нод
    /// </summary>  
    public abstract class MarkdownNode
    {
        public List<InlineNode> Children { get; set; } = new List<InlineNode>();

        public abstract string Render();
    }
}
