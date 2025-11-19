using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Models.SyntaxTreeModels;

namespace Markdown.Models.SyntaxTree
{
    public class SyntaxTree : ISyntaxTree
    {
        /// <summary>
        /// Коллекция корневых узлов абстрактного синтаксического дерева
        /// </summary>
        public List<Node> Tree { get; }

        /// <summary>
        /// Строит абстрактное синтаксическое дерево (AST) из потока токенов.
        /// Анализирует последовательность токенов и создает иерархическую структуру узлов,
        /// отражающую семантическую структуру исходного документа.
        /// </summary>
        /// <param name="tokens">Коллекция токенов, полученная от лексического анализатора</param>
        public SyntaxTree(List<Token> tokens)
        {
            throw new NotImplementedException();
        }
    }
}
