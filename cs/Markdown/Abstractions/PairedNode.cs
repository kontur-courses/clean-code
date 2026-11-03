using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Abstractions
{
    /// <summary>
    /// Парные элементы, хранят значения начала и конца
    /// </summary>
    public abstract class PairedNode : MarkdownNode
    {
        // сохраняют начало и конце данного элемента 
        protected string OpeningTag { get; }
        protected string ClosingTag { get; }

        protected PairedNode(string opening, string closing)
        {
            OpeningTag = opening;
            ClosingTag = closing;
        }

    }

}
