using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Models.SyntaxTreeModels
{
    /// <summary>
    /// Представляет абстрактное синтаксическое дерево (AST) документа
    /// </summary>
    public interface ISyntaxTree
    {
        public List<Node> Tree { get; }
    }
}
