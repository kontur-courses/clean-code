using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Models.SyntaxTree;
using Markdown.Models.SyntaxTreeModels;

namespace Markdown.Entities.Builders
{
    /// <summary>
    /// Преобразует абстрактное синтаксическое дерево (AST) в текст с новой разметкой.
    /// </summary>
    /// <param name="tree">Абстрактное синтаксическое дерево для преобразования</param>
    /// <returns>Текст с измененной разметкой</returns>
    public interface IBuilder
    {
        string Build(ISyntaxTree tree);
    }
}
