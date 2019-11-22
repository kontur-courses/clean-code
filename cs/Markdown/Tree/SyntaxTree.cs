using System.Collections.Generic;
using System.Text;
using Markdown.Languages;

namespace Markdown.Tree
{
    public class SyntaxTree : SyntaxNode, INode
    {
        public SyntaxTree(List<SyntaxNode> childNode) : base(childNode)
        {
        }

        public SyntaxTree() : base(new List<SyntaxNode>())
        {
        }

        public void Add(SyntaxNode node)
        {
            ChildNode.Add(node);
        }

        public override string BuildLinesWithTag(LanguageTagDict languageTagDict)
        {
            var result = new StringBuilder();
            foreach (var child in ChildNode)
            {
                result.Append(child.BuildLinesWithTag(languageTagDict));
            }

            return result.ToString();
        }
    }
}