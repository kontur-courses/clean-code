using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Models;
using Markdown.Models.SyntaxTreeModels;

namespace Markdown.Entities.Builders
{
    /// <summary>
    /// Преобразует абстрактное синтаксическое дерево (AST) в HTML-код.
    /// Выполняет обход дерева в глубину и генерирует соответствующие HTML-теги для каждого узла.
    /// </summary>
    /// <param name="tree">Абстрактное синтаксическое дерево для преобразования</param>
    /// <returns>HTML-код, соответствующий структуре исходного дерева</returns>
    /// <remarks>
    /// Для каждого типа узла AST генерирует соответствующий HTML-тег:
    /// - Заголовки → h1, h2, etc.
    /// - Жирный текст → strong
    /// - Курсив → em
    /// - Параграфы → p
    /// </remarks>
    public class HtmlBuilder : IBuilder
    {
        public string Build(ISyntaxTree tree)
        {
            throw new NotImplementedException();
        }
    }
}
