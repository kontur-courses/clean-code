
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
