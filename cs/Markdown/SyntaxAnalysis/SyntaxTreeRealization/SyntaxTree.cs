using System.Collections.Generic;

namespace Markdown.SyntaxAnalysis.SyntaxTreeRealization
{
    public class SyntaxTree
    {
        public SyntaxTreeNode Root { get; set; }

        public IEnumerable<SyntaxTreeNode> Enumerate()
        {
            return Enumerate(Root);
        }

        private IEnumerable<SyntaxTreeNode> Enumerate(SyntaxTreeNode syntaxTreeNode)
        {
            yield return syntaxTreeNode;

            foreach (var treeNode in syntaxTreeNode.Children)
            {
                foreach (var node in Enumerate(treeNode))
                {
                    yield return node;
                }
            }
        }
    }
}