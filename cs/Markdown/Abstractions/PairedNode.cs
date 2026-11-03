using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Abstractions
{
    /// <summary>
    /// Базовый класс для парных элементов
    /// </summary>
    /// 
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

        //Отдает работа детям, если нет детей должен возращаеть пустую строку
        public override string Render()
        {
            return null;
        }

    }

}
