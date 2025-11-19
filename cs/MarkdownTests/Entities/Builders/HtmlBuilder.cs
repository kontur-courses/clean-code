using System.Text;
using Markdown.Enums;
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
    /// - Заголовки → h1
    /// - Жирный текст → strong
    /// - Курсив → em
    /// - Параграфы → p
    /// </remarks>
    public class HtmlBuilder : IBuilder
    {
        private readonly StringBuilder htmlBuilder = new StringBuilder();
        private readonly Dictionary<NodeType, string> tagMapping = new Dictionary<NodeType, string> 
        {
            { NodeType.Document, "" },
            { NodeType.Paragraph, "p" },
            { NodeType.Header, "h1" },
            { NodeType.Bold, "strong" },
            { NodeType.Italic, "em" },
            { NodeType.Text, "" },
            { NodeType.Link, "a" }
        };

        public string Build(ISyntaxTree tree)
        {
            if (tree == null)
                return string.Empty;

            htmlBuilder.Clear();
            BuildNodes(tree.Tree);
            return htmlBuilder.ToString();
        }

        private void BuildNodes(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                BuildNode(node);
            }
        }

        private void BuildNode(Node node)
        {
            switch (node.Type)
            {
                case NodeType.Text:
                    BuildTextNode(node);
                    break;
                case NodeType.Document:
                    BuildDocumentNode(node);
                    break;
                case NodeType.Link:
                    BuildLinkNode(node);
                    break;
                default:
                    BuildFormattedNode(node);
                    break;
            }
        }

        private void BuildTextNode(Node node)
        {
            if (!string.IsNullOrEmpty(node.Value))
            {
                var escapedText = EscapeHtml(node.Value);
                htmlBuilder.Append(escapedText);
            }
        }

        private void BuildDocumentNode(Node node)
        {
            if (node.ChildrenNodes != null)
            {
                BuildNodes(node.ChildrenNodes);
            }
        }

        private void BuildFormattedNode(Node node)
        {
            var tagName = tagMapping[node.Type];

            if (!string.IsNullOrEmpty(tagName))
            {
                htmlBuilder.Append($"<{tagName}>");
            }

            if (node.ChildrenNodes != null && node.ChildrenNodes.Count > 0)
            {
                BuildNodes(node.ChildrenNodes);
            }
            else if (!string.IsNullOrEmpty(node.Value))
            {
                BuildTextNode(node);
            }

            if (!string.IsNullOrEmpty(tagName))
            {
                htmlBuilder.Append($"</{tagName}>");
            }
        }

        /// <summary>
        /// Строит HTML для узла-ссылки
        /// </summary>
        /// <param name="node">Узел ссылки с атрибутами URL и Title</param>
        private void BuildLinkNode(Node node)
        {
            if (string.IsNullOrEmpty(node.Value))
            {
                if (node.ChildrenNodes != null && node.ChildrenNodes.Count > 0)
                {
                    BuildNodes(node.ChildrenNodes);
                }
                return;
            }

            var url = EscapeHtmlAttribute(node.Value);
            htmlBuilder.Append($"<a href=\"{url}\"");

            if (!string.IsNullOrEmpty(node.Title))
            {
                var title = EscapeHtmlAttribute(node.Title);
                htmlBuilder.Append($" title=\"{title}\"");
            }

            htmlBuilder.Append(">");

            if (node.ChildrenNodes != null && node.ChildrenNodes.Count > 0)
            {
                BuildNodes(node.ChildrenNodes);
            }

            htmlBuilder.Append("</a>");
        }

        private string EscapeHtml(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            // Эскейпинг HTML-символов для безопасности
            return text
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#39;");
        }

        /// <summary>
        /// Эскейпинг для атрибутов HTML (URL и title)
        /// </summary>
        private string EscapeHtmlAttribute(string attribute)
        {
            if (string.IsNullOrEmpty(attribute))
                return attribute;

            // Для атрибутов нужно экранировать кавычки и амперсанды
            return attribute
                .Replace("&", "&amp;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#39;");
        }
    }
}